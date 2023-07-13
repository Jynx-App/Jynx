using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Services.Exceptions;
using Jynx.Validation.Fluent.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal abstract class RepositoryService<TRepository, TEntity> : BaseService, IEntityService<TEntity>
        where TRepository : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly bool _isSoftRemovable;

        // Using memory cache helps cut down on requests to the database, Repository should have lifetime of scoped so this cache should be only per request.
        private readonly Dictionary<string, IEnumerable<TEntity>> _cache = new();

        public TRepository Repository { get; }
        public IValidator<TEntity> Validator { get; }
        public ISystemClock SystemClock { get; }

        public virtual string DefaultNotFoundMessage { get; } = $"{typeof(TEntity).Name} not found";

        protected RepositoryService(
            TRepository repository,
            IValidator<TEntity> validator,
            ISystemClock systemClock,
            ILogger logger)
            : base(logger)
        {
            Repository = repository;
            Validator = validator;
            SystemClock = systemClock;

            _isSoftRemovable = typeof(TEntity).IsAssignableTo(typeof(ISoftRemovableEntity));
        }

        public virtual Task<TEntity> CreateAsync(TEntity entity)
            => CreateAsync(entity, true);

        protected async Task<TEntity> CreateAsync(TEntity entity, bool validateEntity)
        {
            if (validateEntity)
            {
                var (isEntityValid, validationErrors) = await IsValidAsync(entity, ValidationMode.Create);

                if (!isEntityValid)
                    throw new EntityValidationException(typeof(TEntity).Name, validationErrors);
            }

            entity.Created = SystemClock.UtcNow.DateTime;

            return await Repository.CreateAsync(entity);
        }

        public async virtual Task<TEntity?> GetAsync(string id)
        {
            if (TryGetFromCache(id, out var cachedEntities))
                return cachedEntities.FirstOrDefault();

            var entity = await Repository.GetAsync(id);

            if(entity is not null)
                SetInCache(id, entity);

            return entity;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var (isEntityValid, validationErrors) = await IsValidAsync(entity, ValidationMode.Update);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(TEntity).Name, validationErrors);

            entity.Edited = SystemClock.UtcNow.DateTime;

            RemoveFromCache(entity.Id!);

            return await Repository.UpdateAsync(entity);
        }

        public virtual async Task<bool> RemoveAsync(string id)
        {
            if (_isSoftRemovable)
            {
                var entity = await GetAsync(id);

                if (entity is ISoftRemovableEntity softRemovableEntity)
                {
                    softRemovableEntity.Removed = SystemClock.UtcNow.DateTime;

                    return await Repository.UpdateAsync(entity);
                }
            }

            RemoveFromCache(id);

            return await Repository.RemoveAsync(id);
        }

        public virtual Task<bool> ExistsAsync(string id)
            => Repository.ExistsAsync(id);

        public virtual Task<string> UpsertAsync(TEntity entity)
            => Repository.UpsertAsync(entity);

        public virtual void Patch(TEntity target, ICanPatch<TEntity> source)
            => source.Patch(target);

        public async Task<(bool isValid, IEnumerable<string> errorMessages)> IsValidAsync(TEntity entity, ValidationMode validationMode = default)
        {
            var validationResult = await Validator.ValidateAsync(entity, options => options.IncludeRuleSets(ValidationMode.Default.ToString(), validationMode.ToString()));

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);

            return (validationResult.IsValid, errorMessages);
        }

        protected bool TryGetFromCache(string key, out IEnumerable<TEntity> entities)
        {
            if(!_cache.ContainsKey(key))
            {
                entities = Array.Empty<TEntity>();

                return false;
            }

            entities = _cache[key];

            return true;
        }

        protected void SetInCache(string key, TEntity entity)
            => _cache[key] = new[] { entity };

        protected void SetInCache(string key, IEnumerable<TEntity> entity)
            => _cache[key] = entity.ToArray();

        protected void RemoveFromCache(string key)
            => _cache.Remove(key);
    }
}

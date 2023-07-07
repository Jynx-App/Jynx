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

        public virtual async Task<string> CreateAsync(TEntity entity)
        {
            var (isEntityValid, validationErrors) = await IsValidAsync(entity, ValidationMode.Create);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(TEntity).Name, validationErrors);

            return await InternalCreateAsync(entity);
        }

        protected async Task<string> InternalCreateAsync(TEntity entity)
        {
            entity.Created = SystemClock.UtcNow.Date;

            return await Repository.CreateAsync(entity);
        }

        public virtual Task<TEntity?> GetAsync(string id)
            => Repository.GetAsync(id);

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var (isEntityValid, validationErrors) = await IsValidAsync(entity, ValidationMode.Update);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(TEntity).Name, validationErrors);

            entity.Edited = SystemClock.UtcNow.Date;

            return await Repository.UpdateAsync(entity);
        }

        public virtual async Task<bool> RemoveAsync(string id)
        {
            if (_isSoftRemovable)
            {
                var entity = await GetAsync(id);

                if (entity is ISoftRemovableEntity softRemovableEntity)
                {
                    softRemovableEntity.Removed = SystemClock.UtcNow.Date;

                    return await Repository.UpdateAsync(entity);
                }
            }

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
    }
}

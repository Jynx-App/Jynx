using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Jynx.Common.Services.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal abstract class RepositoryService<TRepository, TEntity> : BaseService, IRepositoryService<TEntity>
        where TRepository : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly bool _isSoftRemovable;

        public TRepository Repository { get; }
        public IValidator<TEntity> Validator { get; }
        public ISystemClock SystemClock { get; }

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

        protected virtual string GenerateId(TEntity entity)
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        public virtual async Task<string> CreateAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            var (isEntityValid, validationErrors) = await IsValidAsync(entity);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(TEntity).Name, validationErrors);

            entity.Created = SystemClock.UtcNow.Date;

            return await Repository.CreateAsync(entity);
        }

        public virtual Task<TEntity?> GetAsync(string id)
            => Repository.GetAsync(id);

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            var (isEntityValid, validationErrors) = await IsValidAsync(entity);

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

        public virtual void ExistsAsync(string id)
            => Repository.ExistsAsync(id);

        public virtual void Patch(TEntity target, ICanPatch<TEntity> source)
            => source.Patch(target);

        public async Task<(bool isValid, IEnumerable<string> errorMessages)> IsValidAsync(TEntity entity)
        {
            var validationResult = await Validator.ValidateAsync(entity);

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);

            return (validationResult.IsValid, errorMessages);
        }
    }
}

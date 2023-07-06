using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Jynx.Common.Services.Exceptions;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal abstract class RepositoryService<TRepository, TEntity> : BaseService, IRepositoryService<TEntity>
        where TRepository : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        public TRepository Repository { get; }
        public IValidator<TEntity> Validator { get; }

        protected RepositoryService(
            TRepository repository,
            IValidator<TEntity> validator,
            ILogger logger)
            : base(logger)
        {
            Repository = repository;
            Validator = validator;
        }

        public virtual async Task<string> CreateAsync(TEntity entity)
        {
            var (isEntityValid, validationErrors) = await IsEntityValidAsync(entity);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(TEntity).Name, validationErrors);

            return await Repository.CreateAsync(entity);
        }

        public virtual Task<TEntity?> GetAsync(string id)
            => Repository.GetAsync(id);

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var (isEntityValid, validationErrors) = await IsEntityValidAsync(entity);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(TEntity).Name, validationErrors);

            await Repository.UpdateAsync(entity);
        }

        public virtual Task RemoveAsync(string id)
            => Repository.RemoveAsync(id);

        public virtual void ExistsAsync(string id)
            => Repository.ExistsAsync(id);

        public virtual void Patch(TEntity target, ICanPatch<TEntity> source)
            => source.Patch(target);

        protected async Task<(bool isValid, IEnumerable<string> errorMessages)> IsEntityValidAsync(TEntity entity)
        {
            var validationResult = await Validator.ValidateAsync(entity);

            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);

            return (validationResult.IsValid, errorMessages);
        }
    }
}

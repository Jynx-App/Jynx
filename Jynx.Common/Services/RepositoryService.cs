using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal abstract class RepositoryService<TRepository, TEntity> : BaseService, IRepositoryService<TEntity>
        where TRepository : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        public TRepository Repository { get; }

        protected RepositoryService(
            TRepository repository,
            ILogger logger)
            : base(logger)
        {
            Repository = repository;
        }

        public virtual Task<string> CreateAsync(TEntity entity)
            => Repository.CreateAsync(entity);

        public virtual Task<TEntity?> GetAsync(string id)
            => Repository.GetAsync(id);

        public virtual Task UpdateAsync(TEntity entity)
            => Repository.UpdateAsync(entity);

        public virtual Task RemoveAsync(string id)
            => Repository.RemoveAsync(id);

        public virtual void ExistsAsync(string id)
            => Repository.ExistsAsync(id);

        public virtual void Patch(TEntity target, ICanPatch<TEntity> source)
            => source.Patch(target);
    }
}

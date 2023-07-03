using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal abstract class RepositoryService<TRepository, TModel> : BaseService, IRepositoryService<TModel>
        where TRepository : IRepository<TModel>
        where TModel : BaseEntity
    {
        public TRepository Repository { get; }

        protected RepositoryService(
            TRepository repository,
            ILogger logger)
            : base(logger)
        {
            Repository = repository;
        }

        public virtual Task<string> CreateAsync(TModel entity)
            => Repository.CreateAsync(entity);

        public virtual Task<TModel?> ReadAsync(string id)
            => Repository.ReadAsync(id);

        public virtual Task UpdateAsync(TModel entity)
            => Repository.UpdateAsync(entity);

        public virtual Task DeleteAsync(string id)
            => Repository.DeleteAsync(id);
    }
}

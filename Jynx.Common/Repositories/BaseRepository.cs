using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected BaseRepository(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public abstract Task<string> CreateAsync(TEntity entity);

        public abstract Task<TEntity?> GetAsync(string id);

        public abstract Task<bool> UpdateAsync(TEntity entity);

        public abstract Task<bool> RemoveAsync(string id);

        public abstract Task<bool> ExistsAsync(string id);

        public abstract Task<string> UpsertAsync(TEntity entity);
    }
}

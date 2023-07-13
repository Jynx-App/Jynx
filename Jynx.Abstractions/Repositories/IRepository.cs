using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> RemoveAsync(string id);
        Task<TEntity?> GetAsync(string id);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> ExistsAsync(string compoundId);
        Task<string> UpsertAsync(TEntity entity);
    }
}
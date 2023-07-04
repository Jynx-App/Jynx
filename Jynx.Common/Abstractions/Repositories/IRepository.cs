using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<string> CreateAsync(TEntity entity);
        Task RemoveAsync(string id);
        Task<TEntity?> ReadAsync(string id);
        Task UpdateAsync(TEntity entity);
        Task<bool> ExistsAsync(string compoundId);
    }
}
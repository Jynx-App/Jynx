using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    internal interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<string> CreateAsync(TEntity entity);
        Task<bool> RemoveAsync(string id);
        Task<TEntity?> GetAsync(string id);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> ExistsAsync(string compoundId);
    }
}
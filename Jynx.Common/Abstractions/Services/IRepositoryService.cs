using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IRepositoryService<TEntity> where TEntity : BaseEntity
    {
        Task<string> CreateAsync(TEntity entity);
        Task<bool> RemoveAsync(string id);
        Task<TEntity?> GetAsync(string id);
        Task<bool> UpdateAsync(TEntity entity);
        void Patch(TEntity target, ICanPatch<TEntity> source);
        void ExistsAsync(string id);
    }
}
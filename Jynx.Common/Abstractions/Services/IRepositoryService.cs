using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IRepositoryService<TEntity> where TEntity : BaseEntity
    {
        Task<string> CreateAsync(TEntity entity);
        Task RemoveAsync(string id);
        Task RemoveAsync(TEntity entity);
        Task<TEntity?> ReadAsync(string id);
        Task UpdateAsync(TEntity entity);
        void Patch(TEntity target, ICanPatch<TEntity> source);
    }
}
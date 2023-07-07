using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IRepositoryService<TEntity>
        where TEntity : BaseEntity
    {
        string DefaultNotFoundMessage { get; }

        Task<string> CreateAsync(TEntity entity);
        Task<bool> RemoveAsync(string id);
        Task<TEntity?> GetAsync(string id);
        Task<bool> UpdateAsync(TEntity entity);
        void Patch(TEntity target, ICanPatch<TEntity> source);
        Task<bool> ExistsAsync(string id);
    }
}
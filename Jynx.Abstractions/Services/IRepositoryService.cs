using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IRepositoryService<TEntity> : IRepositoryService
        where TEntity : BaseEntity
    {
        Task<string> CreateAsync(TEntity entity);
        Task<TEntity?> GetAsync(string id);
        Task<bool> UpdateAsync(TEntity entity);
        void Patch(TEntity target, ICanPatch<TEntity> source);
        Task<string> UpsertAsync(TEntity entity);
    }

    public interface IRepositoryService
    {
        string DefaultNotFoundMessage { get; }

        Task<bool> RemoveAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
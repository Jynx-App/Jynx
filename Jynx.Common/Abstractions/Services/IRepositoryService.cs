using Jynx.Common.Entities;
using System.Formats.Tar;

namespace Jynx.Common.Abstractions.Services
{
    public interface IRepositoryService<TEntity> where TEntity : BaseEntity
    {
        Task<string> CreateAsync(TEntity entity);
        Task RemoveAsync(string id);
        Task RemoveAsync(TEntity entity);
        Task<TEntity?> ReadAsync(string id);
        Task UpdateAsync(TEntity entity);
    }
}
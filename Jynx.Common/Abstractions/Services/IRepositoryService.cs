using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IRepositoryService<TModel> where TModel : BaseEntity
    {
        Task<string> CreateAsync(TModel entity);
        Task DeleteAsync(string id);
        Task<TModel> ReadAsync(string id);
        Task UpdateAsync(TModel entity);
    }
}
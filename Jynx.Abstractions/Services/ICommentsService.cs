using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface ICommentsService : IRepositoryService<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId);
    }
}

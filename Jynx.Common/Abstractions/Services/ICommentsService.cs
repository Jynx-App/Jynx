using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface ICommentsService : IRepositoryService<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId);
    }
}

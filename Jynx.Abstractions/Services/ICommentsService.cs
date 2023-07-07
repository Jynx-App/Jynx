using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface ICommentsService : IEntityService<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId);
    }
}

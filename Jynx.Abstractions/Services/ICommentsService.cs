using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface ICommentsService : IEntityService<Comment>
    {
        Task<bool> ClearVoteAsync(string postId, string userId);
        Task<bool> DownVoteAsync(string postId, string userId);
        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId);
        Task<bool> UpVoteAsync(string postId, string userId);
    }
}

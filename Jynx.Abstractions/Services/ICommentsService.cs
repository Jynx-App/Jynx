using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface ICommentsService : IEntityService<Comment>
    {
        Task<bool> ClearVoteAsync(string postId, string userId);
        Task<bool> DownVoteAsync(string postId, string userId);
        Task<IEnumerable<Comment>> GetByPostIdAsync(string postId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore, bool includeRemoved = false);
        Task<IEnumerable<Comment>> GetPinnedByPostIdAsync(string postId, bool includeRemoved = false);
        Task<bool> PinAsync(string id);
        Task<bool> PinAsync(Comment entity);
        Task<bool> UnpinAsync(string id);
        Task<bool> UnpinAsync(Comment entity);
        Task<bool> UpVoteAsync(string postId, string userId);
    }
}

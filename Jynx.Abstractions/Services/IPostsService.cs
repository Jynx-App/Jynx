using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IPostsService : IEntityService<Post>
    {
        Task<bool> ClearVoteAsync(string postId, string userId);
        Task<bool> DownVoteAsync(string postId, string userId);
        Task<IEnumerable<Post>> GetByDistrictIdAsync(string districtId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore);
        Task<IEnumerable<Comment>> GetCommentsAsync(string postId, int count, int offset = 0, PostsSortOrder? sortOrder = PostsSortOrder.HighestScore);
        Task<IEnumerable<Post>> GetPinnedByDistrictIdAsync(string districtId);
        Task<bool> PinAsync(string id);
        Task<bool> PinAsync(Post entity);
        Task<bool> UnpinAsync(string id);
        Task<bool> UnpinAsync(Post entity);
        Task<bool> UpVoteAsync(string postId, string userId);
    }
}

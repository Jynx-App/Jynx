using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IPostsService : IEntityService<Post>
    {
        Task<bool> ClearVoteAsync(string postId, string userId);
        Task<bool> DownVoteAsync(string postId, string userId);
        Task<bool> UpVoteAsync(string postId, string userId);
    }
}

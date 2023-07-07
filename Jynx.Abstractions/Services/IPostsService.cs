using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IPostsService : IRepositoryService<Post>
    {
        Task<bool> VoteAsync(string postId, string userId, bool negative);
    }
}

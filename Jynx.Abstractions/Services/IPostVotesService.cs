using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IPostVotesService : IEntityService<PostVote>
    {
        Task<PostVote?> GetByPostIdAndUserIdAsync(string postId, string userId);
        Task<bool> RemoveByPostIdAndUserIdAsync(string postId, string userId);
    }
}

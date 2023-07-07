using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IPostVotesService : IRepositoryService<PostVote>
    {
        Task<PostVote?> GetByPostIdAndUserIdAsync(string postId, string userId);
    }
}

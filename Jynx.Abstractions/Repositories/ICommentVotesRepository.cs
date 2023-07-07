using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface ICommentVotesRepository : IRepository<CommentVote>
    {
        Task<CommentVote?> GetByCommentIdAndUserIdAsync(string commentId, string userId);
        Task<bool> RemoveByCommentIdAndUserIdAsync(string commentId, string userId);
    }
}

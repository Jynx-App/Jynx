using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface ICommentVotesService : IEntityService<CommentVote>
    {
        Task<CommentVote?> GetByCommentIdAndUserIdAsync(string commentId, string userId);
        Task<bool> RemoveByCommentIdAndUserIdAsync(string commentId, string userId);
    }
}

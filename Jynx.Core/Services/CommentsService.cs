using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class CommentsService : RepositoryService<ICommentsRepository, Comment>, ICommentsService
    {
        private readonly ICommentVotesService _commentVotesService;

        public CommentsService(
            ICommentsRepository repository,
            ICommentVotesService commentVotesService,
            IValidator<Comment> validator,
            ISystemClock systemClock,
            ILogger<CommentsService> logger)
            : base(repository, validator, systemClock, logger)
        {
            _commentVotesService = commentVotesService;
        }

        public Task<IEnumerable<Comment>> GetByPostIdAsync(string postId)
            => Repository.GetByPostIdAsync(postId);

        public async Task<bool> UpVoteAsync(string postId, string userId)
            => await VoteAsync(postId, userId, true);

        public async Task<bool> DownVoteAsync(string postId, string userId)
            => await VoteAsync(postId, userId, false);

        public async Task<bool> ClearVoteAsync(string postId, string userId)
            => await _commentVotesService.RemoveByCommentIdAndUserIdAsync(postId, userId);

        public async Task<bool> VoteAsync(string commentId, string userId, bool up)
        {
            var commentVote = await _commentVotesService.GetByCommentIdAndUserIdAsync(commentId, userId);

            if (commentVote?.Up == up)
                return true;

            var flipped = commentVote?.Up == !up;

            commentVote ??= new CommentVote
            {
                Id = userId,
                CommentId = commentId
            };

            commentVote.Up = up;

            _ = await _commentVotesService.UpsertAsync(commentVote);

            var comment = await GetAsync(commentId);

            if (comment is null)
                return false;

            if (flipped)
            {
                if (up)
                {
                    comment.DownVotes--;
                }
                else
                {
                    comment.UpVotes--;
                }
            }

            if (up)
            {
                comment.UpVotes++;
            }
            else
            {
                comment.DownVotes++;
            }

            var commentedUpdated = await UpdateAsync(comment);

            return commentedUpdated;
        }
    }
}

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

        public Task<IEnumerable<Comment>> GetByPostIdAsync(string commentId)
            => Repository.GetByPostIdAsync(commentId);

        public async Task<bool> UpVoteAsync(string commentId, string userId)
            => await VoteAsync(commentId, userId, true);

        public async Task<bool> DownVoteAsync(string commentId, string userId)
            => await VoteAsync(commentId, userId, false);

        public async Task<bool> ClearVoteAsync(string commentId, string userId)
        {
            var commentVote = await _commentVotesService.GetByCommentIdAndUserIdAsync(commentId, userId);

            if (commentVote is null)
                return false;

            var removed = await _commentVotesService.RemoveAsync(commentVote.Id!);

            if (!removed)
                return false;

            var comment = await GetAsync(commentId);

            if (comment is null)
                return false;

            if (commentVote.Up)
            {
                comment.UpVotes--;
            }
            else
            {
                comment.DownVotes--;
            }

            var updated = await UpdateAsync(comment);

            return updated;
        }

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

            var updated = await UpdateAsync(comment);

            return updated;
        }
    }
}

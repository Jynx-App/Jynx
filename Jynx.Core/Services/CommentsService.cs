using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Jynx.Abstractions.Services;
using Jynx.Common.Events;
using Jynx.Core.Services.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class CommentsService : RepositoryService<ICommentsRepository, Comment>, ICommentsService
    {
        private readonly ICommentVotesService _commentVotesService;
        private readonly IEventPublisher _eventPublisher;

        public CommentsService(
            ICommentsRepository repository,
            ICommentVotesService commentVotesService,
            IValidator<Comment> validator,
            ISystemClock systemClock,
            IEventPublisher eventPublisher,
            ILogger<CommentsService> logger)
            : base(repository, validator, systemClock, logger)
        {
            _commentVotesService = commentVotesService;
            _eventPublisher = eventPublisher;
        }

        public override async Task<Comment> CreateAsync(Comment entity)
        {
            var creatingEvent = new CreatingCommentEvent(entity);

            await _eventPublisher.PublishAsync(this, creatingEvent);

            if (creatingEvent.Canceled)
                throw new TaskCanceledException();

            entity = await base.CreateAsync(entity);

            await _eventPublisher.PublishAsync(this, new CreatedCommentEvent(entity));

            return entity;
        }

        public Task<IEnumerable<Comment>> GetByPostIdAsync(string commentId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore, bool includeRemoved = false)
            => Repository.GetByPostIdAsync(commentId, count, offset, sortOrder);

        public Task<IEnumerable<Comment>> GetPinnedByPostIdAsync(string postId, bool includeRemoved = false)
            => Repository.GetPinnedByPostIdAsync(postId);

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

        public async Task<bool> PinAsync(string id)
            => await PinAsync(await GetAsync(id) ?? throw new NotFoundException(nameof(Post)));

        public async Task<bool> PinAsync(Comment entity)
        {
            if (entity.Pinned is not null)
                return true;

            var pinnedComments = await GetPinnedByPostIdAsync(entity.PostId);

            var @event = new PinningCommentEvent(entity, true, pinnedComments.Count());

            await _eventPublisher.PublishAsync(this, @event);

            if (@event.Canceled)
                return false;

            entity.Pinned = SystemClock.UtcNow.DateTime;

            var updated = await UpdateAsync(entity);

            return updated;
        }

        public async Task<bool> UnpinAsync(string id)
            => await UnpinAsync(await GetAsync(id) ?? throw new NotFoundException(nameof(Post)));

        public async Task<bool> UnpinAsync(Comment entity)
        {
            if (entity.Pinned is null)
                return true;

            var pinnedComments = await GetPinnedByPostIdAsync(entity.PostId);

            var @event = new PinningCommentEvent(entity, false, pinnedComments.Count());

            await _eventPublisher.PublishAsync(this, @event);

            if (@event.Canceled)
                return false;

            entity.Pinned = null;

            var updated = await UpdateAsync(entity);

            return updated;
        }
    }
}

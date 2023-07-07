using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class PostsService : RepositoryService<IPostsRepository, Post>, IPostsService
    {
        private readonly IPostVotesService _postVotesService;

        public PostsService(
            IPostsRepository postRepository,
            IPostVotesService postVotesService,
            IValidator<Post> validator,
            ISystemClock systemClock,
            ILogger<PostsService> logger)
            : base(postRepository, validator, systemClock, logger)
        {
            _postVotesService = postVotesService;
        }

        public async Task<bool> UpVoteAsync(string postId, string userId)
            => await VoteAsync(postId, userId, true);

        public async Task<bool> DownVoteAsync(string postId, string userId)
            => await VoteAsync(postId, userId, false);

        public async Task<bool> ClearVoteAsync(string postId, string userId)
        {
            var postVote = await _postVotesService.GetByPostIdAndUserIdAsync(postId, userId);

            if (postVote is null)
                return false;

            var removed = await _postVotesService.RemoveAsync(postVote.Id!);

            if (!removed)
                return false;

            var post = await GetAsync(postId);

            if(post is null)
                return false;

            if(postVote.Up)
            {
                post.UpVotes--;
            }
            else
            {
                post.DownVotes--;
            }

            var updated = await UpdateAsync(post);

            return updated;
        }

        public async Task<bool> VoteAsync(string postId, string userId, bool up)
        {
            var postVote = await _postVotesService.GetByPostIdAndUserIdAsync(postId, userId);

            if (postVote?.Up == up)
                return true;

            var flipped = postVote?.Up == !up;

            postVote ??= new PostVote
            {
                Id = userId,
                PostId = postId
            };

            postVote.Up = up;

            _ = await _postVotesService.UpsertAsync(postVote);

            var post = await GetAsync(postId);

            if (post is null)
                return false;

            if (flipped)
            {
                if (up)
                {
                    post.DownVotes--;
                }
                else
                {
                    post.UpVotes--;
                }
            }

            if (up)
            {
                post.UpVotes++;
            }
            else
            {
                post.DownVotes++;
            }

            var updated = await UpdateAsync(post);

            return updated;
        }

    }
}

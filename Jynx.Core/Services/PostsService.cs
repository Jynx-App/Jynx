using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class PostsService : RepositoryService<IPostsRepository, Post>, IPostsService
    {
        private readonly IUsersService _usersService;
        private readonly IPostVotesService _postVotesService;

        public PostsService(
            IPostsRepository postRepository,
            IUsersService usersService,
            IPostVotesService postVotesService,
            IValidator<Post> validator,
            ISystemClock systemClock,
            ILogger<PostsService> logger)
            : base(postRepository, validator, systemClock, logger)
        {
            _usersService = usersService;
            _postVotesService = postVotesService;
        }

        public async Task<bool> VoteAsync(string postId, string userId, bool negative)
        {
            if (!await _usersService.ExistsAsync(userId))
                throw new NotFoundException(nameof(User));

            if (!await Repository.ExistsAsync(postId))
                throw new NotFoundException(nameof(Post));

            var postVote = await _postVotesService.GetByPostIdAndUserIdAsync(postId, userId);

            postVote ??= new PostVote
            {
                Id = userId,
                PostId = postId
            };

            postVote.Negative = negative;

            _ = await _postVotesService.UpsertAsync(postVote);

            return true;
        }
    }
}

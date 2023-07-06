using FluentValidation;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class PostsService : RepositoryService<IPostsRepository, Post>, IPostsService
    {
        public string DefaultLockedMessage => "Post is locked, no new comments can be created";

        public PostsService(
            IPostsRepository postRepository,
            IValidator<Post> validator,
            ISystemClock systemClock,
            ILogger<PostsService> logger)
            : base(postRepository, validator, systemClock, logger)
        {

        }
    }
}

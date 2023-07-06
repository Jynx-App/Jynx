using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
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

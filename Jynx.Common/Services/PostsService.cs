using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class PostsService : RepositoryService<IPostsRepository, Post>, IPostsService
    {
        public PostsService(
            IPostsRepository postRepository,
            IValidator<Post> validator,
            ILogger<PostsService> logger)
            : base(postRepository, validator, logger)
        {

        }
    }
}

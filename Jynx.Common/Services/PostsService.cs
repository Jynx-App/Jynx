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
            ILogger<PostsService> logger)
            : base(postRepository, logger)
        {

        }
    }
}

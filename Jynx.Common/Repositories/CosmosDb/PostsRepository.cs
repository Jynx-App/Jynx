using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class PostsRepository : BaseCosmosDbRepository<Post>, IPostsRepository
    {
        public PostsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ILogger<PostsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Posts"
        };
    }
}

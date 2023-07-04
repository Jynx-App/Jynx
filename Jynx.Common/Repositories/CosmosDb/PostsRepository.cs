using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class PostsRepository : CosmosDbRepository<Post>, IPostsRepository
    {
        public PostsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<PostsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Posts"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(Post.DistrictId);

        protected override string GetCompoundId(Post entity)
            => CosmosDbRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);
    }
}

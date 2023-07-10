using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class PostsRepository : CosmosRepositoryWithCompoundId<Post>, IPostsRepository
    {
        public PostsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<PostsRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Posts",
            PartitionKey = nameof(Post.DistrictId)
        };

        public async Task<IEnumerable<Post>> GetByDistrictIdAsync(string districtId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore)
        {
            var queryString = $@"
                SELECT *
                FROM c
                WHERE c.districtId = @districtId
                {GetSortSqlString(sortOrder)}
                OFFSET {offset} LIMIT {count}
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@districtId", districtId);

            var entities = await ExecuteQueryAsync(query);

            return entities;
        }
    }
}

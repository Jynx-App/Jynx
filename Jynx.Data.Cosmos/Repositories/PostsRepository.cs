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
                AND (NOT IS_DEFINED(c.pinned) OR IS_NULL(c.pinned))
                {GetSortSqlString(sortOrder)}
                OFFSET {offset} LIMIT {count}
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@districtId", districtId);

            var entities = await ExecuteQueryAsync(query);

            FixIds(entities);

            return entities;
        }

        public async Task<IEnumerable<Post>> GetPinnedByDistrictIdAsync(string districtId)
        {
            var queryString = $@"
                SELECT *
                FROM c
                WHERE c.districtId = @districtId
                AND (IS_DEFINED(c.pinned) AND NOT IS_NULL(c.pinned))
                ORDER BY c.pinned DESC
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@districtId", districtId);

            var entities = await ExecuteQueryAsync(query);

            FixIds(entities);

            return entities;
        }
    }
}

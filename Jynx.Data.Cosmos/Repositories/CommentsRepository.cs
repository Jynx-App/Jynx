using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class CommentsRepository : CosmosRepositoryWithCompoundId<Comment>, ICommentsRepository, IDistrictRelatedRepository
    {
        public CommentsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<CommentsRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Comments",
            PartitionKey = nameof(Comment.PostId)
        };

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore, bool includeRemoved = false)
        {
            var removeQueryString = !includeRemoved ? "AND (NOT IS_DEFINED(c.removed) OR IS_NULL(c.removed))" : "";

            var queryString = $@"
                SELECT *
                FROM c
                WHERE c.postId = @postId
                AND (NOT IS_DEFINED(c.pinned) OR IS_NULL(c.pinned))
                {removeQueryString}
                {GetSortSqlString(sortOrder)}
                OFFSET {offset} LIMIT {count}
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@postId", compoundPostId);

            var entities = await ExecuteQueryAsync(query);

            FixIds(entities);

            return entities;
        }

        public string GetDistrictId(string id)
            => id.Split('.').FirstOrDefault()!;

        public async Task<IEnumerable<Comment>> GetPinnedByPostIdAsync(string compoundPostId, bool includeRemoved = false)
        {
            var removeQueryString = !includeRemoved ? "AND (NOT IS_DEFINED(c.removed) OR IS_NULL(c.removed))" : "";

            var queryString = $@"
                SELECT *
                FROM c
                WHERE c.postId = @postId
                AND (IS_DEFINED(c.pinned) AND NOT IS_NULL(c.pinned))
                {removeQueryString}
                ORDER BY c.pinned DESC
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@postId", compoundPostId);

            var entities = await ExecuteQueryAsync(query);

            FixIds(entities);

            return entities;
        }
    }
}

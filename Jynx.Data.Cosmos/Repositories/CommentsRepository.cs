using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class CommentsRepository : CosmosRepositoryWithCompoundId<Comment>, ICommentsRepository
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

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore)
        {
            var queryString = $@"
                SELECT *
                FROM c
                WHERE c.postId = @postId
                {GetSortSqlString(sortOrder)}
                OFFSET {offset} LIMIT {count}
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@postId", compoundPostId);

            var entities = await ExecuteQueryAsync(query);

            FixIds(entities);

            return entities;
        }
    }
}

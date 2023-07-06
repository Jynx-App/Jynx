using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
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

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId)
        {
            var (_, postPk) = GetIdAndPartitionKeyFromCompoundKey(compoundPostId);

            var query = new QueryDefinition("SELECT * FROM c WHERE c.postId = @postId")
                .WithParameter("@postId", compoundPostId);

            var entities = await ExecuteQueryAsync(query);

            foreach (var entity in entities)
            {
                entity.Id = GetCompoundId(entity);
            }

            return entities;
        }
    }
}

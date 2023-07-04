using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class CommentsRepository : CosmosRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger<CommentsRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Comments"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(Comment.DistrictId);

        protected override string GetCompoundId(Comment entity)
            => CosmosRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId)
        {
            var (_, postPk) = CosmosRepositoryUtility.GetIdAndPartitionKeyFromCompoundKey(compoundPostId);

            var query = new QueryDefinition("SELECT * FROM c WHERE c.postId = @postId AND c.districtId = @postPk")
                .WithParameter("@postId", compoundPostId)
                .WithParameter("@postPk", postPk);

            var entities = await ExecuteQueryAsync(query);

            foreach (var entity in entities)
            {
                entity.Id = GetCompoundId(entity);
            }

            return entities;
        }
    }
}

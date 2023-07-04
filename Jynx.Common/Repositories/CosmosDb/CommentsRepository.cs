using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class CommentsRepository : CosmosDbRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<CommentsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Comments"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(Comment.DistrictId);

        protected override string GetCompoundId(Comment entity)
            => CosmosDbRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId)
        {
            var (_, postPk) = CosmosDbRepositoryUtility.GetIdAndPartitionKeyFromCompoundKey(compoundPostId);

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

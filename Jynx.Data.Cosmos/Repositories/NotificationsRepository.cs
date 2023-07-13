using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Common.Chronography;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class NotificationsRepository : CosmosRepositoryWithCompoundId<Notification>, INotificationsRepository
    {
        public NotificationsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<NotificationsRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Notifications",
            PartitionKey = nameof(Notification.UserId)
        };

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId, DateTime since, int limit = 100, int offset = 0)
        {
            var queryString = $@"
                SELECT * FROM c
                WHERE c.userId = @userId
                AND c.created >= '{since.ToIso8601String()}'
                ORDER BY c.created DESC
                OFFSET {offset} LIMIT {limit}
            ";

            var query = new QueryDefinition(queryString)
                .WithParameter("@userId", userId);

            var results = await ExecuteQueryAsync(query);

            return results;
        }
    }
}

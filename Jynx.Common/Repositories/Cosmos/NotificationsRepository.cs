using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class NotificationsRepository : CosmosRepository<Notification>, INotificationsRepository
    {
        public NotificationsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger<NotificationsRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Notifications"
        };

        protected override string GetCompoundId(Notification entity)
            => CreateCompoundId(entity.UserId, entity.Id!);

        protected override string GetPartitionKeyValue(Notification entity)
            => CreatePartitionKeyHash(entity.UserId);
    }
}

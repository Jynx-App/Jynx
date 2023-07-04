using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class NotificationsRepository : CosmosDbRepository<Notification>, INotificationsRepository
    {
        public NotificationsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<NotificationsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Notifications"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(Notification.UserId);

        protected override string GetCompoundId(Notification entity)
            => CosmosDbRepositoryUtility.CreateCompoundId(entity.UserId, entity.Id!);
    }
}

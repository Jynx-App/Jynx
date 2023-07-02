using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class NotificationsRepository : BaseCosmosDbRepository<Notification>, INotificationsRepository
    {
        public NotificationsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ILogger<NotificationsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Notifications"
        };
    }
}

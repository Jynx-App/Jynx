using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.CosmosDb;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class NotificationsService : RepositoryService<NotificationsRepository, Notification>, INotificationsService
    {
        public NotificationsService(
            NotificationsRepository repository,
            ILogger logger)
            : base(repository, logger)
        {
        }
    }
}

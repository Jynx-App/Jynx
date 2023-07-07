using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class NotificationsService : RepositoryService<INotificationsRepository, Notification>, INotificationsService
    {
        public NotificationsService(
            INotificationsRepository repository,
            ISystemClock systemClock,
            ILogger<NotificationsService> logger)
            : base(repository, new NotificationValidator(), systemClock, logger)
        {
        }
    }
}

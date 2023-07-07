using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class NotificationsService : RepositoryService<INotificationsRepository, Notification>, INotificationsService
    {
        public NotificationsService(
            INotificationsRepository repository,
            IValidator<Notification> validator,
            ISystemClock systemClock,
            ILogger<NotificationsService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }
    }
}

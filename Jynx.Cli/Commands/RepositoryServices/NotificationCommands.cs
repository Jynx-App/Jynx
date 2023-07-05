using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("notifications")]
    internal class NotificationCommands : RepositoryServiceCommands<INotificationsService, Notification>
    {
        public NotificationCommands(INotificationsService service)
            : base(service)
        {
        }
    }
}

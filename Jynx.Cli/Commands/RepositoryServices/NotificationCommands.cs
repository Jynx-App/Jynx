using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

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

using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("notifications")]
    internal class NotificationsCommands : EntityServiceCommands<INotificationsService, Notification>
    {
        public NotificationsCommands(INotificationsService service)
            : base(service)
        {
        }
    }
}

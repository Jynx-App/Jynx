﻿using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("notifications")]
    internal class NotificationCommands : EntityServiceCommands<INotificationsService, Notification>
    {
        public NotificationCommands(INotificationsService service)
            : base(service)
        {
        }
    }
}
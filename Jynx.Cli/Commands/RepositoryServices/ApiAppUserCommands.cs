using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("api-app-users")]
    internal class ApiAppUserCommands : RepositoryServiceCommands<IApiAppUsersService, ApiAppUser>
    {
        public ApiAppUserCommands(IApiAppUsersService repository)
            : base(repository)
        {
        }
    }
}

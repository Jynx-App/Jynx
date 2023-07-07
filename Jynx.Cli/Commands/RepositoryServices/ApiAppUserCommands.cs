using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

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

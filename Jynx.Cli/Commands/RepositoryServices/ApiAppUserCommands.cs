using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

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

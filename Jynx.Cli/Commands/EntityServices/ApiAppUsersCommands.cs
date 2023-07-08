using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("api-app-users")]
    internal class ApiAppUsersCommands : EntityServiceCommands<IApiAppUsersService, ApiAppUser>
    {
        public ApiAppUsersCommands(IApiAppUsersService repository)
            : base(repository)
        {
        }
    }
}

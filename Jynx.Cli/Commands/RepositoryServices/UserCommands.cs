using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("users")]
    internal class UserCommands : RepositoryServiceCommands<IUsersService, User>
    {
        public UserCommands(IUsersService service)
            : base(service)
        {
        }
    }
}

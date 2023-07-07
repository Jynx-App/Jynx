using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

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

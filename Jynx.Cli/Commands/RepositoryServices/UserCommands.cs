using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

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

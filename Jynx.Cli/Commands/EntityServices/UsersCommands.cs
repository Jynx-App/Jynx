using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("users")]
    internal class UsersCommands : EntityServiceCommands<IUsersService, User>
    {
        public UsersCommands(IUsersService service)
            : base(service)
        {
        }
    }
}

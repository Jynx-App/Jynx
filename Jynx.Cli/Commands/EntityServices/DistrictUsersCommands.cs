using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("district-users")]
    internal class DistrictUsersCommands : EntityServiceCommands<IDistrictUsersService, DistrictUser>
    {
        public DistrictUsersCommands(IDistrictUsersService service)
            : base(service)
        {
        }
    }
}

using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("district-users")]
    internal class DistrictUserCommands : RepositoryServiceCommands<IDistrictUsersService, DistrictUser>
    {
        public DistrictUserCommands(IDistrictUsersService service)
            : base(service)
        {
        }
    }
}

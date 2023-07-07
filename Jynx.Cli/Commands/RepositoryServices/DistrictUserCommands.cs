using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

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

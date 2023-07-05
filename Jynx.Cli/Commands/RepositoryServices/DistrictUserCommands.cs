using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

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

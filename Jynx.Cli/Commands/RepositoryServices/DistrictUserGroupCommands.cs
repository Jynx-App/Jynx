using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("district-user-group")]
    internal class DistrictUserGroupCommands : RepositoryServiceCommands<IDistrictUserGroupsService, DistrictUserGroup>
    {
        public DistrictUserGroupCommands(IDistrictUserGroupsService service)
            : base(service)
        {
        }
    }
}

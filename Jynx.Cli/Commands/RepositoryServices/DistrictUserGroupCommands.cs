using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

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

using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("district-user-group")]
    internal class DistrictUserGroupsCommands : EntityServiceCommands<IDistrictUserGroupsService, DistrictUserGroup>
    {
        public DistrictUserGroupsCommands(IDistrictUserGroupsService service)
            : base(service)
        {
        }
    }
}

using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("district-user-group")]
    internal class DistrictUserGroupCommands : EntityServiceCommands<IDistrictUserGroupsService, DistrictUserGroup>
    {
        public DistrictUserGroupCommands(IDistrictUserGroupsService service)
            : base(service)
        {
        }
    }
}

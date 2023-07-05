using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

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

using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("districts")]
    internal class DistrictCommands : RepositoryServiceCommands<IDistrictsService, District>
    {
        public DistrictCommands(IDistrictsService service)
            : base(service)
        {
        }
    }
}

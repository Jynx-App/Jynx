using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

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

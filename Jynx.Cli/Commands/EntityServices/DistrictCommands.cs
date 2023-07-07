using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("districts")]
    internal class DistrictCommands : EntityServiceCommands<IDistrictsService, District>
    {
        public DistrictCommands(IDistrictsService service)
            : base(service)
        {
        }
    }
}

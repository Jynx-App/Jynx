using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("districts")]
    internal class DistrictsCommands : EntityServiceCommands<IDistrictsService, District>
    {
        public DistrictsCommands(IDistrictsService service)
            : base(service)
        {
        }
    }
}

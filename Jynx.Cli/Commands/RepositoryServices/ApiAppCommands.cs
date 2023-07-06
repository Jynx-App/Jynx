using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("api-apps")]
    internal class ApiAppCommands : RepositoryServiceCommands<IApiAppService, ApiApp>, IDisposable
    {
        public ApiAppCommands(IApiAppService repository)
            : base(repository)
        {
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

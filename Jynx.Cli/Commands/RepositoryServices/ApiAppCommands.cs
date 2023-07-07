using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("api-apps")]
    internal class ApiAppCommands : RepositoryServiceCommands<IApiAppsService, ApiApp>, IDisposable
    {
        public ApiAppCommands(IApiAppsService repository)
            : base(repository)
        {
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

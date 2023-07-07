using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("api-apps")]
    internal class ApiAppCommands : EntityServiceCommands<IApiAppsService, ApiApp>, IDisposable
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

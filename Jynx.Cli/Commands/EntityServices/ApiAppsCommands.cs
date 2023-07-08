using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("api-apps")]
    internal class ApiAppsCommands : EntityServiceCommands<IApiAppsService, ApiApp>, IDisposable
    {
        public ApiAppsCommands(IApiAppsService repository)
            : base(repository)
        {
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

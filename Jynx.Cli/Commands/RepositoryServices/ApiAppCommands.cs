using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

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

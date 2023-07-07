using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal abstract class BaseService
    {
        public ILogger Logger { get; }

        public BaseService(ILogger logger)
        {
            Logger = logger;
        }
    }
}

using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class ApiAppsRepository : CosmosRepository<ApiApp>, IApiAppsRepository
    {
        public ApiAppsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger<ApiAppsRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "ApiApps"
        };
    }
}

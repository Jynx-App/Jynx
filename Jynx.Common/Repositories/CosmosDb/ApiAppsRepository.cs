using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class ApiAppsRepository : CosmosDbRepository<ApiApp>, IApiAppsRepository
    {
        public ApiAppsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<ApiAppsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "ApiApps"
        };
    }
}

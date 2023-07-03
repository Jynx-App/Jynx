using Jynx.Common.Abstractions.Chronometry;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class ApiAppsRepository : BaseCosmosDbRepository<ApiApp>, IApiAppsRepository
    {
        public ApiAppsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            IDateTimeService dateTimeService,
            ILogger<ApiAppsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, dateTimeService, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "ApiApps"
        };
    }
}

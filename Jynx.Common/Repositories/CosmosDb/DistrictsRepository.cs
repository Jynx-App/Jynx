using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class DistrictsRepository : BaseCosmosDbRepository<District>, IDistrictsRepository
    {
        public DistrictsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ILogger<DistrictsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Districts"
        };
    }
}

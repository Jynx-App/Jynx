using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.Cosmos.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class DistrictsRepository : CosmosRepository<District>, IDistrictsRepository
    {
        public DistrictsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger<DistrictsRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Districts"
        };

        protected override string GenerateId(District entity)
            => throw new GenerateIdException();
    }
}

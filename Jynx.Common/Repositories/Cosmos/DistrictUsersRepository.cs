using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class DistrictUsersRepository : CosmosRepositoryWithCompoundId<DistrictUser>, IDistrictUsersRepository
    {
        public DistrictUsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<DistrictUsersRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {

        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUsers"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(DistrictUser.DistrictId);
    }
}

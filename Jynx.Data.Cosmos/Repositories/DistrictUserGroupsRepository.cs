using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class DistrictUserGroupsRepository : CosmosRepositoryWithCompoundId<DistrictUserGroup>, IDistrictUserGroupsRepository
    {
        public DistrictUserGroupsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<DistrictUserGroupsRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUserGroups",
            PartitionKey = nameof(DistrictUserGroup.DistrictId)
        };
    }
}

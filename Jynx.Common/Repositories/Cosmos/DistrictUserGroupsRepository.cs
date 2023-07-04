using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class DistrictUserGroupsRepository : CosmosRepository<DistrictUserGroup>, IDistrictUserGroupsRepository
    {
        public DistrictUserGroupsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger<DistrictUserGroupsRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUserGroups"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(DistrictUserGroup.DistrictId);

        protected override string GetCompoundId(DistrictUserGroup entity)
            => CosmosRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);
    }
}

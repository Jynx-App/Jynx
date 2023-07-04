using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class DistrictUserGroupsRepository : CosmosDbRepository<DistrictUserGroup>, IDistrictUserGroupsRepository
    {
        public DistrictUserGroupsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<DistrictUserGroupsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUserGroups"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(DistrictUserGroup.DistrictId);

        protected override string GetCompoundId(DistrictUserGroup entity)
            => CosmosDbRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);
    }
}

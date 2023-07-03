using Jynx.Common.Abstractions.Chronometry;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class DistrictUserGroupsRepository : BaseCosmosDbRepository<DistrictUserGroup>, IDistrictUserGroupsRepository
    {
        public DistrictUserGroupsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            IDateTimeService dateTimeService,
            ILogger<DistrictUserGroupsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, dateTimeService, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUserGroups"
        };
    }
}

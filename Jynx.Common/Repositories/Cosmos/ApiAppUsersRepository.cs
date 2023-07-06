using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class ApiAppUsersRepository : CosmosRepositoryWithCompoundId<ApiAppUser>, IApiAppUsersRepository
    {
        public ApiAppUsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<ApiAppUsersRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "ApiAppUsers",
            PartitionKey = nameof(ApiAppUser.ApiAppId)
        };
    }
}

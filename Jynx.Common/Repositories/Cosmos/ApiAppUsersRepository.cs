using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
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
            ISystemClock systemClock,
            ILogger<ApiAppUsersRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "ApiAppUsers"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(ApiAppUser.ApiAppId);
    }
}

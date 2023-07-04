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
    internal class DistrictUsersRepository : CosmosRepository<DistrictUser>, IDistrictUsersRepository
    {
        public DistrictUsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger<DistrictUsersRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {

        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUsers"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(DistrictUser.DistrictId);

        protected override string GenerateId(DistrictUser entity)
            => throw new GenerateIdException(); // Id should be same as Id of User entity

        protected override string GetCompoundId(DistrictUser entity)
            => CosmosRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);
    }
}

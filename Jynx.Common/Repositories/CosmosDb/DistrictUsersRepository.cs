using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.CosmosDb.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class DistrictUsersRepository : CosmosDbRepository<DistrictUser>, IDistrictUsersRepository
    {
        public DistrictUsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<DistrictUsersRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {

        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUsers"
        };

        protected override string GetPartitionKeyPropertyName()
            => nameof(DistrictUser.DistrictId);

        protected override string GenerateId(DistrictUser entity)
            => throw new GenerateIdException(); // Id should be same as Id of User entity

        protected override string GetCompoundId(DistrictUser entity)
            => CosmosDbRepositoryUtility.CreateCompoundId(entity.DistrictId, entity.Id!);
    }
}

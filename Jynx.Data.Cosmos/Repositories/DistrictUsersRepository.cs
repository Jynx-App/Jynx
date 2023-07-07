using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
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
            Name = "DistrictUsers",
            PartitionKey = nameof(DistrictUser.DistrictId)
        };

        protected override string GenerateId(DistrictUser entity)
            => throw new GenerateIdException(); // Id should be same as Id of User entity

        public async Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId)
        {
            var compoundId = CreateCompoundId(districtId, userId);

            return await GetAsync(compoundId);
        }
    }
}

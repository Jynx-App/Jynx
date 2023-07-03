using Jynx.Common.Abstractions.Chronometry;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class DistrictUsersRepository : BaseCosmosDbRepository<DistrictUser>, IDistrictUsersRepository
    {
        public DistrictUsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            IDateTimeService dateTimeService,
            ILogger<DistrictUsersRepository> logger)
            : base(cosmosClient, cosmosDbOptions, dateTimeService, logger)
        {

        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "DistrictUsers"
        };
    }
}

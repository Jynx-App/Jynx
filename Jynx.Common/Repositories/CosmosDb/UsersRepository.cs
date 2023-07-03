using Jynx.Common.Abstractions.Chronometry;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class UsersRepository : BaseCosmosDbRepository<Entities.User>, IUsersRepository
    {
        public UsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            IDateTimeService dateTimeService,
            ILogger<UsersRepository> logger)
            : base(cosmosClient, cosmosDbOptions, dateTimeService, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Users"
        };
    }
}

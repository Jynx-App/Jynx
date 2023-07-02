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
            ILogger<UsersRepository> logger)
            : base(cosmosClient, cosmosDbOptions, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Users"
        };
    }
}

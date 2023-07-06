using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class UsersRepository : CosmosRepository<Entities.User>, IUsersRepository
    {
        public UsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<UsersRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Users"
        };

        public override Task<string> CreateAsync(Entities.User entity)
            => base.CreateAsync(entity);

        public async Task<Entities.User?> GetByUsernameAsync(string username)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.username = @username")
                .WithParameter("@username", username);

            var results = await ExecuteQueryAsync(query);

            return results.FirstOrDefault();
        }

        public async Task<bool> IsUsernameUsed(string username)
        {
            var query = new QueryDefinition("SELECT id FROM c WHERE c.username = @username")
                .WithParameter("@username", username);

            var results = await ExecuteQueryAsync(query);

            return results.Any();
        }
    }
}

using Jynx.Abstractions.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User = Jynx.Abstractions.Entities.User;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class UsersRepository : CosmosRepository<User>, IUsersRepository
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

        public override Task<string> CreateAsync(User entity)
            => base.CreateAsync(entity);

        public async Task<User?> GetByUsernameAsync(string username)
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

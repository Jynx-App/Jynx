using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Repositories.Cosmos.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal class UsersRepository : CosmosRepository<Entities.User>, IUsersRepository
    {
        private readonly IPasswordHasher<Entities.User> _passwordHasher;

        public UsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            IPasswordHasher<Entities.User> passwordHasher,
            ILogger<UsersRepository> logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
            _passwordHasher = passwordHasher;
        }

        protected override CosmosContainerInfo ContainerInfo => new()
        {
            Name = "Users"
        };

        public override Task<string> CreateAsync(Entities.User entity)
        {
            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            return base.CreateAsync(entity);
        }

        public async Task UpdatePasswordAsync(string userId, string newPassword)
        {
            var user = await GetAsync(userId) ?? throw new NotFoundException();

            user.Password = _passwordHasher.HashPassword(user, newPassword);

            await UpdateAsync(user);
        }

        public async Task<Entities.User?> GetByUsernameAsync(string username)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.username = @username")
                .WithParameter("@username", username);

            var results = await ExecuteQueryAsync(query);

            return results.FirstOrDefault();
        }
    }
}

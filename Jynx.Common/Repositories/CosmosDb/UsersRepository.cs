using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Repositories.CosmosDb.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class UsersRepository : CosmosDbRepository<Entities.User>, IUsersRepository
    {
        private readonly IPasswordHasher<Entities.User> _passwordHasher;

        public UsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            IPasswordHasher<Entities.User> passwordHasher,
            ILogger<UsersRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
            _passwordHasher = passwordHasher;
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
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
            var user = await ReadAsync(userId) ?? throw new NotFoundException();

            user.Password = _passwordHasher.HashPassword(user, newPassword);

            await UpdateAsync(user);
        }
    }
}

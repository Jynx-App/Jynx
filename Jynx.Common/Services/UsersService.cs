using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Jynx.Common.Services.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class UsersService : RepositoryService<IUsersRepository, User>, IUsersService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersService(
            IUsersRepository repository,
            IValidator<User> validator,
            ISystemClock systemClock,
            IPasswordHasher<User> passwordHasher,
            ILogger<UsersService> logger)
            : base(repository, validator, systemClock, logger)
        {
            _passwordHasher = passwordHasher;
        }

        public async override Task<string> CreateAsync(User entity)
        {
            var (isEntityValid, validationErrors) = await IsValidAsync(entity, ValidationMode.Create);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(User).Name, validationErrors);

            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            return await InternalCreateAsync(entity);
        }

        public async Task UpdatePasswordAsync(string id, string newPassword)
        {
            var entity = await GetAsync(id);

            if (entity is null)
                return;

            entity.Password = _passwordHasher.HashPassword(entity, newPassword);

            await UpdateAsync(entity);
        }

        public Task<User?> GetByUsernameAsync(string username)
            => Repository.GetByUsernameAsync(username);

        public Task<bool> IsUsernameUnique(string username)
            => Repository.IsUsernameUnique(username);
    }
}

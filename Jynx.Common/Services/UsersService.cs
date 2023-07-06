﻿using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
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

        public override Task<string> CreateAsync(User entity)
        {
            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            return base.CreateAsync(entity);
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

        public Task<bool> IsUsernameUsed(string username)
            => Repository.IsUsernameUsed(username);
    }
}

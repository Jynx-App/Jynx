using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Services.Exceptions;
using Jynx.Validation.Fluent.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class UsersService : RepositoryService<IUsersRepository, User>, IUsersService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly INotificationsService _notificationsService;

        public UsersService(
            IUsersRepository repository,
            IValidator<User> validator,
            ISystemClock systemClock,
            IPasswordHasher<User> passwordHasher,
            INotificationsService notificationsService,
            ILogger<UsersService> logger)
            : base(repository, validator, systemClock, logger)
        {
            _passwordHasher = passwordHasher;
            _notificationsService = notificationsService;
        }

        public async override Task<User> CreateAsync(User entity)
        {
            var (isEntityValid, validationErrors) = await IsValidAsync(entity, ValidationMode.Create);

            if (!isEntityValid)
                throw new EntityValidationException(typeof(User).Name, validationErrors);

            entity.Password = _passwordHasher.HashPassword(entity, entity.Password);

            return await CreateAsync(entity, false);
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

        public async Task<IEnumerable<Notification>> GetNotifications(string userId, DateTime? since = null, int limit = 100, int offset = 0)
        {
            var notifications = await _notificationsService.GetByUserIdAsync(userId, since, limit, offset);

            return notifications;
        }
    }
}

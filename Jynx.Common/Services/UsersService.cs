using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class UsersService : RepositoryService<IUsersRepository, User>, IUsersService
    {
        public UsersService(
            IUsersRepository repository,
            IValidator<User> validator,
            ILogger<UsersService> logger)
            : base(repository, validator, logger)
        {
        }

        public Task<User?> GetByUsernameAsync(string username)
            => Repository.GetByUsernameAsync(username);
    }
}

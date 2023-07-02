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
            ILogger<UsersService> logger)
            : base(repository, logger)
        {
        }
    }
}

using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class ApiAppUsersService : RepositoryService<IApiAppUsersRepository, ApiAppUser>, IApiAppUsersService
    {
        public ApiAppUsersService(
            IApiAppUsersRepository repository,
            IValidator<ApiAppUser> validator,
            ILogger<ApiAppUsersService> logger)
            : base(repository, validator, logger)
        {
        }
    }
}

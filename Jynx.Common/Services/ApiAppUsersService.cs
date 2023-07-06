using FluentValidation;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class ApiAppUsersService : RepositoryService<IApiAppUsersRepository, ApiAppUser>, IApiAppUsersService
    {
        public ApiAppUsersService(
            IApiAppUsersRepository repository,
            IValidator<ApiAppUser> validator,
            ISystemClock systemClock,
            ILogger<ApiAppUsersService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }
    }
}

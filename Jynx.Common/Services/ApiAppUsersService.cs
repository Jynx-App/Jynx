using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class ApiAppUsersService : RepositoryService<IApiAppUsersRepository, ApiAppUser>, IApiAppUsersService
    {
        public ApiAppUsersService(
            IApiAppUsersRepository repository,
            ISystemClock systemClock,
            ILogger<ApiAppUsersService> logger)
            : base(repository, new ApiAppUserValidator(), systemClock, logger)
        {
        }
    }
}

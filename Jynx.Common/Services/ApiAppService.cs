using FluentValidation;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class ApiAppService : RepositoryService<IApiAppsRepository, ApiApp>, IApiAppService
    {
        public ApiAppService(
            IApiAppsRepository repository,
            IValidator<ApiApp> validator,
            ISystemClock systemClock,
            ILogger<ApiAppService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }
    }
}

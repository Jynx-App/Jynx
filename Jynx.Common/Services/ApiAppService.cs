using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class ApiAppService : RepositoryService<IApiAppsRepository, ApiApp>, IApiAppService
    {
        public ApiAppService(
            IApiAppsRepository repository,
            IValidator<ApiApp> validator,
            ILogger<ApiAppService> logger)
            : base(repository, validator, logger)
        {
        }
    }
}

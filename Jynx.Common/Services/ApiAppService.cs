using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class ApiAppService : RepositoryService<IApiAppsRepository, ApiApp>, IApiAppsService
    {
        public ApiAppService(
            IApiAppsRepository repository,
            ISystemClock systemClock,
            ILogger<ApiAppService> logger)
            : base(repository, new ApiAppValidator(), systemClock, logger)
        {
        }

        public override async Task<string> CreateAsync(ApiApp entity)
        {
            entity.PublicKey = GenerateBase64Guid();

            entity.PrivateKey = GenerateBase64Guid();

            return await base.CreateAsync(entity);
        }

        private static string GenerateBase64Guid()
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());
    }
}

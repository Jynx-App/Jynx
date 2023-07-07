using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictUsersService : RepositoryService<IDistrictUsersRepository, DistrictUser>, IDistrictUsersService
    {
        public DistrictUsersService(
            IDistrictUsersRepository repository,
            ISystemClock systemClock,
            ILogger<DistrictUsersService> logger)
            : base(repository, new DistrictUserValidator(), systemClock, logger)
        {
        }

        public Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId)
            => Repository.GetByDistrictIdAndUserId(districtId, userId);
    }
}

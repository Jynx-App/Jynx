using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class DistrictUsersService : RepositoryService<IDistrictUsersRepository, DistrictUser>, IDistrictUsersService
    {
        public DistrictUsersService(
            IDistrictUsersRepository repository,
            IValidator<DistrictUser> validator,
            ISystemClock systemClock,
            ILogger<DistrictUsersService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }

        public Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId)
            => Repository.GetByDistrictIdAndUserId(districtId, userId);
    }
}

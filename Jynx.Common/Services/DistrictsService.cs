using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictsService : RepositoryService<IDistrictsRepository, District>, IDistrictsService
    {
        public DistrictsService(
            IDistrictsRepository districtRepository,
            ILogger<DistrictsService> logger)
            : base(districtRepository, logger)
        {

        }

        public async Task<bool> IsUserAllowedToPostAsync(string districtId, string userId)
        {
            var district = await ReadAsync(districtId);

            if (district is null)
                return false;

            //Todo: Logic for preventing users from posting to districts they're banned/suspended from goes here

            return true;
        }
    }
}

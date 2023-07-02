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

        public District GetEntityById(string id)
        {
            return new District()
            {
                Id = id,
                Name = "Tulsa",
                Description = "Tulsa, Ok -- District"
            };
        }
    }
}

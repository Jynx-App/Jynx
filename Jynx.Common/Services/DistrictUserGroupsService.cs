using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictUserGroupsService : RepositoryService<IDistrictUserGroupsRepository, DistrictUserGroup>, IDistrictUserGroupsService
    {
        public DistrictUserGroupsService(
            IDistrictUserGroupsRepository repository,
            ILogger<DistrictUserGroupsService> logger)
            : base(repository, logger)
        {
        }
    }
}

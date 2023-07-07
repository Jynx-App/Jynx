using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictUserGroupsService : RepositoryService<IDistrictUserGroupsRepository, DistrictUserGroup>, IDistrictUserGroupsService
    {
        public DistrictUserGroupsService(
            IDistrictUserGroupsRepository repository,
            ISystemClock systemClock,
            ILogger<DistrictUserGroupsService> logger)
            : base(repository, new DistrictUserGroupValidator(), systemClock, logger)
        {
        }
    }
}

using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictUsersService : RepositoryService<IDistrictUsersRepository, DistrictUser>, IDistrictUsersService
    {
        public DistrictUsersService(
            IDistrictUsersRepository repository,
            ILogger logger)
            : base(repository, logger)
        {
        }
    }
}

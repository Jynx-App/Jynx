using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    internal interface IDistrictUsersRepository : IRepository<DistrictUser>
    {
        Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId);
    }
}

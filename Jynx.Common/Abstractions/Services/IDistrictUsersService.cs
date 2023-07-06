using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IDistrictUsersService : IRepositoryService<DistrictUser>
    {
        Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId);
    }
}

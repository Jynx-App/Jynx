using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IDistrictUsersService : IEntityService<DistrictUser>
    {
        Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId);
    }
}

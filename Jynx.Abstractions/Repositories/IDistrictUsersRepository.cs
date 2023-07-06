using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface IDistrictUsersRepository : IRepository<DistrictUser>
    {
        Task<DistrictUser?> GetByDistrictIdAndUserId(string districtId, string userId);
    }
}

using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface IPostsRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetByDistrictIdAsync(string districtId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore, bool includeRemoved = false);
        Task<IEnumerable<Post>> GetPinnedByDistrictIdAsync(string districtId, bool includeRemoved = false);
    }
}

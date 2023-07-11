using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;

namespace Jynx.Abstractions.Services
{
    public interface IDistrictsService : IEntityService<District>
    {
        string DefaultNotAllowedToPostMessage { get; }
        string DefaultNotAllowedToCommentMessage { get; }

        Task<string> CreateAndAssignModerator(District district, string userId);
        Task<bool> DoesUserHavePermissionAsync(string districtId, string userId, ModerationPermission permission);
        Task<IEnumerable<Post>> GetPostsAsync(string districtId, int count, int offset = 0, PostsSortOrder? sortOrder = null);
        Task<bool> IsUserAllowedToPostAndCommentAsync(string districtId, string userId);
        Task<bool> PinAsync(string districtId, string postId);
        Task<bool> UnpinAsync(string districtId, string postId);
    }
}
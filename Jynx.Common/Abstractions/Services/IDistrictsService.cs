using Jynx.Common.Auth;
using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IDistrictsService : IRepositoryService<District>
    {
        Task<bool> DoesUserHavePermissionAsync(string districtId, string userId, ModerationPermission permission);
        Task<bool> IsUserAllowedToPostAndCommentAsync(string districtId, string userId);
    }
}
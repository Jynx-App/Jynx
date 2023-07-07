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
        Task<bool> IsUserAllowedToPostAndCommentAsync(string districtId, string userId);
    }
}
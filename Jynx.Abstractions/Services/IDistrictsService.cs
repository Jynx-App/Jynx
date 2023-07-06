using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;

namespace Jynx.Abstractions.Services
{
    public interface IDistrictsService : IRepositoryService<District>
    {
        string DefaultNotAllowedToPostMessage { get; }
        string DefaultNotAllowedToCommentMessage { get; }

        Task<bool> DoesUserHavePermissionAsync(string districtId, string userId, ModerationPermission permission);
        Task<bool> IsUserAllowedToPostAndCommentAsync(string districtId, string userId);
    }
}
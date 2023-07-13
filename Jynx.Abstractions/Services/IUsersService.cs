using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IUsersService : IEntityService<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<Notification>> GetNotifications(string userId, DateTime? since = null, int limit = 100, int offset = 0);
        Task<bool> IsUsernameUnique(string username);
        Task UpdatePasswordAsync(string id, string newPassword);
    }
}

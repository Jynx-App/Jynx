using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface INotificationsService : IEntityService<Notification>
    {
        public Task<IEnumerable<Notification>> GetByUserIdAsync(string userId, DateTime? since = null, int limit = 100, int offset = 0);
    }
}

using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface INotificationsRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId, DateTime since, int limit = 100, int offset = 0);
    }
}

using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> IsUsernameUsed(string username);
    }
}

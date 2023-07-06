using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    internal interface IUsersRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> IsUsernameUsed(string username);
    }
}

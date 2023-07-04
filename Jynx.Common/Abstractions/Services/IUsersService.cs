using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IUsersService : IRepositoryService<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}

using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IUsersService : IEntityService<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> IsUsernameUnique(string username);
        Task UpdatePasswordAsync(string id, string newPassword);
    }
}

﻿using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> ReadByUsernameAsync(string username);
    }
}

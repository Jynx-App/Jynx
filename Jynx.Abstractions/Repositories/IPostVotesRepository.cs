﻿using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface IPostVotesRepository : IRepository<PostVote>
    {
        Task<PostVote?> GetByPostIdAndUserIdAsync(string postId, string userId);
        Task<bool> RemoveByPostIdAndUserIdAsync(string postId, string userId);
    }
}

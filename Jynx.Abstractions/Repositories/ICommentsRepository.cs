using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Repositories
{
    public interface ICommentsRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId, int count, int offset = 0, PostsSortOrder sortOrder = PostsSortOrder.HighestScore);
    }
}

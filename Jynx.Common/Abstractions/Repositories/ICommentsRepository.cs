using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    public interface ICommentsRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId);
    }
}

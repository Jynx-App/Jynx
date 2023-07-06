using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Repositories
{
    internal interface ICommentsRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByPostIdAsync(string compoundPostId);
    }
}

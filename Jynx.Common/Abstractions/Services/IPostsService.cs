using Jynx.Common.Entities;

namespace Jynx.Common.Abstractions.Services
{
    public interface IPostsService : IRepositoryService<Post>
    {
        string DefaultLockedMessage { get; }
    }
}

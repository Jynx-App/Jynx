using Jynx.Abstractions.Entities;

namespace Jynx.Abstractions.Services
{
    public interface IPostsService : IRepositoryService<Post>
    {
        string DefaultLockedMessage { get; }
    }
}

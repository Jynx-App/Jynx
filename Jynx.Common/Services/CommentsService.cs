using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class CommentsService : RepositoryService<ICommentsRepository, Comment>, ICommentsService
    {
        public CommentsService(
            ICommentsRepository repository,
            ISystemClock systemClock,
            ILogger<CommentsService> logger)
            : base(repository, new CommentValidator(), systemClock, logger)
        {
        }

        public Task<IEnumerable<Comment>> GetByPostIdAsync(string postId)
            => Repository.GetByPostIdAsync(postId);
    }
}

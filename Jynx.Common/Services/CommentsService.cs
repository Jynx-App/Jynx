using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class CommentsService : RepositoryService<ICommentsRepository, Comment>, ICommentsService
    {
        public CommentsService(
            ICommentsRepository repository,
            IValidator<Comment> validator,
            ILogger<CommentsService> logger)
            : base(repository, validator, logger)
        {
        }

        public Task<IEnumerable<Comment>> GetByPostIdAsync(string postId)
            => Repository.GetByPostIdAsync(postId);
    }
}

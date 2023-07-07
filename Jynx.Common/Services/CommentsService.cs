using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class CommentsService : RepositoryService<ICommentsRepository, Comment>, ICommentsService
    {
        public CommentsService(
            ICommentsRepository repository,
            IValidator<Comment> validator,
            ISystemClock systemClock,
            ILogger<CommentsService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }

        public Task<IEnumerable<Comment>> GetByPostIdAsync(string postId)
            => Repository.GetByPostIdAsync(postId);
    }
}

using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class CommentVotesService : RepositoryService<ICommentVotesRepository, CommentVote>, ICommentVotesService
    {
        public CommentVotesService(
            ICommentVotesRepository repository,
            IValidator<CommentVote> validator,
            ISystemClock systemClock,
            ILogger<CommentVotesService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }

        public Task<bool> RemoveByCommentIdAndUserIdAsync(string commentId, string userId)
            => Repository.RemoveByCommentIdAndUserIdAsync(commentId, userId);

        public Task<CommentVote?> GetByCommentIdAndUserIdAsync(string commentId, string userId)
            => Repository.GetByCommentIdAndUserIdAsync(commentId, userId);
    }
}

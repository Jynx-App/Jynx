using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class PostVotesService : RepositoryService<IPostVotesRepository, PostVote>, IPostVotesService
    {
        public PostVotesService(
            IPostVotesRepository repository,
            IValidator<PostVote> validator,
            ISystemClock systemClock,
            ILogger<PostVotesService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }

        public Task<PostVote?> GetByPostIdAndUserIdAsync(string postId, string userId)
            => Repository.GetByPostIdAndUserIdAsync(postId, userId);
    }
}

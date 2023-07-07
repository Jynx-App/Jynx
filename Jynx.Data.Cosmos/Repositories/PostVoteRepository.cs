using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class PostVoteRepository : CosmosRepositoryWithCompoundId<PostVote>, IPostVotesRepository
    {
        public PostVoteRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<PostVoteRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new CosmosContainerInfo
        {
            Name = "PostVotes",
            PartitionKey = nameof(PostVote.PostId)
        };

        protected override string GenerateId(PostVote entity)
            => throw new GenerateIdException(); // Id should be same as Id of User entity

        public async Task<PostVote?> GetByPostIdAndUserIdAsync(string postId, string userId)
        {
            var compoundId = CreateCompoundId(postId, userId);

            return await GetAsync(compoundId);
        }
    }
}

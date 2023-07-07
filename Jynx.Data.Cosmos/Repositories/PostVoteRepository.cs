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
            => throw new UnableToGenerateIdException(); // Id should be same as Id of User entity

        public async Task<bool> RemoveByPostIdAndUserIdAsync(string postId, string userId)
        {
            return await InternalRemoveAsync(userId, postId);
        }

        public async Task<PostVote?> GetByPostIdAndUserIdAsync(string postId, string userId)
        {
            var entity = await InternalGetAsync(userId, postId);

            if (entity is null)
                return null;

            entity.Id = CreateCompoundId(postId, userId);

            return entity;
        }
    }
}

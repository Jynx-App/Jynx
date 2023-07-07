using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal class CommentVotesRepository : CosmosRepository<CommentVote>, ICommentVotesRepository
    {
        public CommentVotesRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger<PostVoteRepository> logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

        protected override CosmosContainerInfo ContainerInfo => new CosmosContainerInfo
        {
            Name = "CommentVotes",
            PartitionKey = nameof(CommentVote.CommentId)
        };

        protected override string GenerateId(CommentVote entity)
            => throw new UnableToGenerateIdException(); // Id should be same as Id of User entity

        public async Task<bool> RemoveByCommentIdAndUserIdAsync(string commentId, string userId)
        {
            return await InternalRemoveAsync(userId, commentId);
        }

        public async Task<CommentVote?> GetByCommentIdAndUserIdAsync(string commentId, string userId)
        {
            return await InternalGetAsync(userId, commentId);
        }
    }
}

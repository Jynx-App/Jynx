using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class CommentsRepository : BaseCosmosDbRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ILogger<CommentsRepository> logger)
            : base(cosmosClient, cosmosDbOptions, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "Comments"
        };
    }
}

using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.Cosmos.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.Cosmos
{
    internal abstract class CosmosRepositoryWithCompoundId<TEntity> : CosmosRepository<TEntity>
        where TEntity : BaseEntity
    {
        public CosmosRepositoryWithCompoundId(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger logger)
            : base(cosmosClient, CosmosOptions, systemClock, logger)
        {
        }

        protected override string GetPartitionKeyPropertyName()
            => "pk";

        public override async Task<string> CreateAsync(TEntity entity)
        {
            var partitionKey = GetPartitionKey(entity);

            var id = await InternalCreateAsync(entity, partitionKey);

            var compoundId = CreateCompoundId(partitionKey, id);

            return compoundId;
        }

        public override async Task<TEntity?> GetAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            var entity = await InternalGetAsync(id, pk);

            if(entity is not null)
                entity.Id = compoundId;

            return entity;
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(entity.Id!);

            entity.Id = id;

            await InternalUpdateAsync(entity, pk);
        }

        public override async Task RemoveAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            await InternalRemoveAsync(id, pk);
        }

        public override async Task<bool> ExistsAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            return await InternalExistsAsync(id, pk);
        }

        protected static string CreateCompoundId(params string[] parts)
            => string.Join(".", parts);

        protected static (string id, string pk) GetIdAndPartitionKeyFromCompoundKey(string compoundId)
        {
            var parts = compoundId.Split(".");

            if (parts.Length != 2)
                throw new InvalidCompoundIdException();

            return (parts[1], parts[0]);
        }

        protected string GetCompoundId(TEntity entity)
            => CreateCompoundId(GetPartitionKey(entity), entity.Id!);
    }
}

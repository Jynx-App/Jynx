using Jynx.Abstractions.Entities;
using Jynx.Data.Cosmos.Repositories.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Data.Cosmos.Repositories
{
    internal abstract class CosmosRepositoryWithCompoundId<TEntity> : CosmosRepository<TEntity>
        where TEntity : BaseEntity
    {
        public CosmosRepositoryWithCompoundId(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger logger)
            : base(cosmosClient, CosmosOptions, logger)
        {
        }

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

            if (entity is not null)
                entity.Id = GetCompoundId(entity);

            return entity;
        }

        public override async Task<bool> UpdateAsync(TEntity entity)
        {
            string pk;

            if (IsCompoundId(entity.Id!))
            {
                (var id, pk) = GetIdAndPartitionKeyFromCompoundKey(entity.Id!);

                entity.Id = id;
            }
            else
            {
                pk = GetPartitionKey(entity);
            }

            return await InternalUpdateAsync(entity, pk);
        }

        public override async Task<bool> RemoveAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            return await InternalRemoveAsync(id, pk);
        }

        public override async Task<bool> ExistsAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            return await InternalExistsAsync(id, pk);
        }

        public override async Task<string> UpsertAsync(TEntity entity)
        {
            string pk;

            if(IsCompoundId(entity.Id!))
            {
                (var id, pk) = GetIdAndPartitionKeyFromCompoundKey(entity.Id!);

                entity.Id = id;
            }
            else
            {
                pk = GetPartitionKey(entity);
            }

            return await InternalUpsertAsync(entity, pk);
        }

        protected static string CreateCompoundId(params string[] parts)
            => string.Join(".", parts);

        protected static (string id, string pk) GetIdAndPartitionKeyFromCompoundKey(string compoundId)
        {
            var parts = compoundId.Split(".");

            if (parts.Length < 2)
                throw new InvalidCompoundIdException();

            var id = parts.Last();

            var pk = string.Join('.', parts.Take(parts.Length - 1));

            return (id, pk);
        }

        protected string GetCompoundId(TEntity entity)
            => CreateCompoundId(GetPartitionKey(entity), entity.Id!);

        protected static bool IsCompoundId(string id)
            => id.Contains('.');
    }
}

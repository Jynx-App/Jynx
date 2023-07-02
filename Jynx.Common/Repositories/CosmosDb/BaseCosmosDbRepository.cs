using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.CosmosDb.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal abstract class BaseCosmosDbRepository<TEntity> : BaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private Container _container;

        protected abstract CosmosDbContainerInfo ContainerInfo { get; }

        protected BaseCosmosDbRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ILogger logger)
            : base(logger)
        {
            _container = cosmosClient.GetContainer(cosmosDbOptions.Value.DatabaseName, ContainerInfo.Name);
        }

        protected virtual string GenerateId(TEntity entity)
            => Guid.NewGuid().ToString();

        protected virtual PartitionKey ResolvePartitionKey(TEntity entity)
            => new(entity.Id);

        public override async Task<string> CreateAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            if(entity.Created == DateTime.MinValue)
                entity.Created = DateTime.UtcNow;

            try
            {
                var result = await _container.CreateItemAsync(entity, ResolvePartitionKey(entity));

                return result.Resource.Id!;
            }
            catch (CosmosException ex)
            {
                if(ex.StatusCode == HttpStatusCode.Conflict)
                    throw new DuplicateEntityException(ex.Message);

                throw;
            }

            throw new Exception();
        }

        public override async Task<TEntity?> ReadAsync(string compoundId)
        {
            try
            {
                var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

                var response = await _container.ReadItemAsync<TEntity>(id, new PartitionKey(pk));

                return response.Resource;
            }
            catch(CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                throw;
            }
        }

        public override Task UpdateAsync(TEntity entity)
        {
            throw new Exception();
        }

        public override async Task DeleteAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(pk));
        }

        public override Task DeleteAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new Exception("Can not delete an entity without an id");

            return DeleteAsync(entity.Id);
        }

        private static (string id, string pk) GetIdAndPartitionKeyFromCompoundKey(string compoundId)
        {
            var parts = compoundId.Split(".");

            if (parts.Length == 2)
                return (parts[0], parts[1]);

            return (compoundId, compoundId);
        }
    }
}

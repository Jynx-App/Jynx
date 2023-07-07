using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Jynx.Data.Cosmos.Repositories.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Jynx.Data.Cosmos.Repositories
{
    internal abstract class CosmosRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly Container _container;

        protected CosmosRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger logger)
        {
            _container = cosmosClient.GetContainer(CosmosOptions.Value.DatabaseName, ContainerInfo.Name);

            if (CosmosOptions.Value.CreateContainersIfMissing)
            {
                Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosOptions.Value.DatabaseName).Result;

                database.CreateContainerIfNotExistsAsync(
                    ContainerInfo.Name,
                    $"/{GetPartitionKeyFieldName()}",
                    ContainerInfo.Throughput).Wait();
            }
            Logger = logger;
        }

        protected abstract CosmosContainerInfo ContainerInfo { get; }

        protected ILogger Logger { get; }

        protected virtual string GenerateId(TEntity entity)
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        public virtual async Task<string> CreateAsync(TEntity entity)
            => await InternalCreateAsync(entity, GetPartitionKey(entity));

        protected async Task<string> InternalCreateAsync(TEntity entity, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            try
            {
                var result = await _container.CreateItemAsync(entity, new PartitionKey(partitionKey));

                return result.Resource.Id!;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Conflict)
                    throw new DuplicateEntityException(ex);

                throw;
            }
        }

        public virtual Task<TEntity?> GetAsync(string id)
            => InternalGetAsync(id, id);

        protected async Task<TEntity?> InternalGetAsync(string id, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<TEntity>(id, new PartitionKey(partitionKey));

                if (response.Resource is ISoftRemovableEntity softRemovableEntity && softRemovableEntity.Removed is not null)
                    return null;

                return response.Resource;
            }
            catch (CosmosException ex)
            {
                return null;
            }
        }

        public virtual Task<bool> UpdateAsync(TEntity entity)
            => InternalUpdateAsync(entity, GetPartitionKey(entity));

        protected async Task<bool> InternalUpdateAsync(TEntity entity, string partitionKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.Id))
                    throw new InvalidIdException();

                var result = await _container.ReplaceItemAsync(entity, entity.Id, new PartitionKey(partitionKey));

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    throw new NotFoundException(entity.GetType().Name, ex);

                Logger.LogError(ex, null);

                return false;
            }
        }

        public virtual Task<bool> RemoveAsync(string id)
            => InternalRemoveAsync(id, id);

        protected async Task<bool> InternalRemoveAsync(string id, string partitionKey)
        {
            var result = await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(partitionKey));

            return result.StatusCode == HttpStatusCode.NoContent;
        }

        public virtual Task<bool> ExistsAsync(string id)
            => InternalExistsAsync(id, id);

        protected async Task<bool> InternalExistsAsync(string id, string partitionKey)
        {
            var partitionKeyFieldName = GetPartitionKeyFieldName();

            var query = new QueryDefinition($"SELECT c.id FROM c WHERE c.id = @id AND c.{partitionKeyFieldName} = @pk")
                .WithParameter("@id", id)
                .WithParameter("@pk", partitionKey);

            var result = await ExecuteQueryAsync(query);

            return result.Any(e => e is not ISoftRemovableEntity se || se.Removed is null);
        }

        protected async Task<IEnumerable<TEntity>> ExecuteQueryAsync(QueryDefinition queryDefinition)
        {
            var query = _container.GetItemQueryIterator<TEntity>(queryDefinition);

            var results = new List<TEntity>();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ReadNextAsync());
            }

            return results;
        }

        public virtual async Task<string> UpsertAsync(TEntity entity)
            => await InternalUpsertAsync(entity, GetPartitionKey(entity));

        protected async Task<string> InternalUpsertAsync(TEntity entity, string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            var result = await _container.UpsertItemAsync(entity, new PartitionKey(partitionKey));

            return result.Resource.Id!;
        }

        protected string GetPartitionKey(TEntity entity)
            => GetPartitionKeyPropertyInfo()?.GetValue(entity) as string ?? throw new MissingPartitionKeyException();

        private PropertyInfo? GetPartitionKeyPropertyInfo()
            => typeof(TEntity).GetProperty(ContainerInfo.PartitionKey);

        private string GetPartitionKeyFieldName()
            => JsonNamingPolicy.CamelCase.ConvertName(ContainerInfo.PartitionKey);
    }
}

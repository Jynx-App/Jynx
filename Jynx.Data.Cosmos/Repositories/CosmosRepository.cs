using Jynx.Abstractions.Entities;
using Jynx.Common.Repositories;
using Jynx.Common.Repositories.Exceptions;
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
    internal abstract class CosmosRepository<TEntity> : BaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly Container _container;

        protected CosmosRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ILogger logger)
            : base(logger)
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
        }

        protected abstract CosmosContainerInfo ContainerInfo { get; }

        protected virtual string GenerateId(TEntity entity)
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        public override async Task<string> CreateAsync(TEntity entity)
            => await InternalCreateAsync(entity, null);

        protected async Task<string> InternalCreateAsync(TEntity entity, string? partitionKey)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            partitionKey ??= GetPartitionKey(entity);

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

        public override Task<TEntity?> GetAsync(string id)
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

        public override Task<bool> UpdateAsync(TEntity entity)
            => InternalUpdateAsync(entity, null);

        protected async Task<bool> InternalUpdateAsync(TEntity entity, string? partitionKey)
        {
            partitionKey ??= GetPartitionKey(entity);

            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new InvalidIdException();

            if (!await InternalExistsAsync(entity.Id, partitionKey))
                throw new NotFoundException(entity.GetType().Name);

            var result = await _container.UpsertItemAsync(entity, new PartitionKey(partitionKey));

            return result.StatusCode == HttpStatusCode.OK;
        }

        public override Task<bool> RemoveAsync(string id)
            => InternalRemoveAsync(id, id);

        protected async Task<bool> InternalRemoveAsync(string id, string partitionKey)
        {
            var result = await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(partitionKey));

            return result.StatusCode == HttpStatusCode.OK;
        }

        public override Task<bool> ExistsAsync(string id)
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

        protected string GetPartitionKey(TEntity entity)
            => GetPartitionKeyPropertyInfo()?.GetValue(entity) as string ?? throw new MissingPartitionKeyException();

        private PropertyInfo? GetPartitionKeyPropertyInfo()
            => typeof(TEntity).GetProperty(ContainerInfo.PartitionKey);

        private string GetPartitionKeyFieldName()
            => JsonNamingPolicy.CamelCase.ConvertName(ContainerInfo.PartitionKey);
    }
}

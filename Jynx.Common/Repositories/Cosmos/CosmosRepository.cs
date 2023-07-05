using Jynx.Common.Azure.Cosmos;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.Cosmos.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;

namespace Jynx.Common.Repositories.Cosmos
{
    internal abstract class CosmosRepository<TEntity> : BaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly Container _container;
        private readonly ISystemClock _systemClock;
        private readonly bool _isSoftRemovable;

        protected CosmosRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosOptions> CosmosOptions,
            ISystemClock systemClock,
            ILogger logger)
            : base(logger)
        {
            _isSoftRemovable = typeof(TEntity).IsAssignableTo(typeof(ISoftRemovableEntity));
            _container = cosmosClient.GetContainer(CosmosOptions.Value.DatabaseName, ContainerInfo.Name);
            _systemClock = systemClock;
        }

        protected abstract CosmosContainerInfo ContainerInfo { get; }

        protected virtual string GetPartitionKeyPropertyName()
            => nameof(BaseEntity.Id);

        protected virtual string GenerateId(TEntity entity)
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        public override async Task<string> CreateAsync(TEntity entity)
            => await InternalCreateAsync(entity, null);

        protected async Task<string> InternalCreateAsync(TEntity entity, string? partitionKey)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            entity.Created = _systemClock.UtcNow.Date;

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

        public override Task UpdateAsync(TEntity entity)
            => InternalUpdateAsync(entity, null);

        protected async Task InternalUpdateAsync(TEntity entity, string? partitionKey)
        {
            partitionKey ??= GetPartitionKey(entity);

            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new InvalidIdException();

            if (!(await InternalExistsAsync(entity.Id, partitionKey)))
                throw new NotFoundException();

            entity.Edited = _systemClock.UtcNow.Date;

            await _container.UpsertItemAsync(entity, new PartitionKey(partitionKey));
        }

        public override Task RemoveAsync(string id)
            => InternalRemoveAsync(id, id);

        protected async Task InternalRemoveAsync(string id, string partitionKey)
        {
            if (_isSoftRemovable)
            {
                var entity = await InternalGetAsync(id, partitionKey);

                if (entity is ISoftRemovableEntity softRemovableEntity)
                {
                    softRemovableEntity.Removed = _systemClock.UtcNow.Date;

                    await _container.UpsertItemAsync(entity, new PartitionKey(partitionKey));

                    return;
                }
            }

            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(partitionKey));
        }

        public override Task<bool> ExistsAsync(string id)
            => InternalExistsAsync(id, id);

        protected async Task<bool> InternalExistsAsync(string id, string partitionKey)
        {
            var partitionKeyPropertyName = GetPartitionKeyPropertyName();

            var query = new QueryDefinition($"SELECT c.id FROM c WHERE c.id = @id AND c.{partitionKeyPropertyName} = @pk")
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
            => GetPartitionKeyPropertyInfo()?.GetValue(entity) as string;

        private PropertyInfo? GetPartitionKeyPropertyInfo()
            => typeof(TEntity).GetProperty(GetPartitionKeyPropertyName());
    }
}

using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Jynx.Common.Repositories.CosmosDb.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal abstract class CosmosDbRepository<TEntity> : BaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly Container _container;
        private readonly ISystemClock _systemClock;
        private readonly bool _isSoftRemovable;

        protected CosmosDbRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger logger)
            : base(logger)
        {
            _isSoftRemovable = typeof(TEntity).IsAssignableTo(typeof(ISoftRemovableEntity));
            _container = cosmosClient.GetContainer(cosmosDbOptions.Value.DatabaseName, ContainerInfo.Name);
            _systemClock = systemClock;

            var type = GetType();

            var resolvePartitionKeyMethodInfo = type?.GetMethod(nameof(GetPartitionKeyPropertyName), BindingFlags.NonPublic | BindingFlags.Instance);

            UsesCompoundId = resolvePartitionKeyMethodInfo?.DeclaringType == type;
        }

        protected bool UsesCompoundId { get; }

        protected abstract CosmosDbContainerInfo ContainerInfo { get; }

        protected virtual string GenerateId(TEntity entity)
            => Guid.NewGuid().ToString().Replace("-", "");

        protected virtual string GetPartitionKeyPropertyName()
            => nameof(BaseEntity.Id);

        protected virtual string GetCompoundId(TEntity entity)
            => entity.Id!;

        public override async Task<string> CreateAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            entity.Created = _systemClock.UtcNow.Date;

            try
            {
                var result = await _container.CreateItemAsync(entity, GetPartitionKey(entity));

                var compoundId = GetCompoundId(result.Resource);

                return compoundId;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Conflict)
                    throw new DuplicateEntityException(ex);

                throw;
            }
        }

        public override async Task<TEntity?> GetAsync(string compoundId)
        {
            try
            {
                var (id, pk) = GetIdAndPartitionKey(compoundId);

                var response = await _container.ReadItemAsync<TEntity>(id, new PartitionKey(pk));

                if (response.Resource is ISoftRemovableEntity softRemovableEntity && softRemovableEntity.Removed is not null)
                    return null;

                response.Resource.Id = GetCompoundId(response.Resource);

                return response.Resource;
            }
            catch (CosmosException ex)
            {
                return null;
            }
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new InvalidIdException();

            if (!(await ExistsAsync(entity.Id)))
                throw new NotFoundException();

            var (id, pk) = GetIdAndPartitionKey(entity.Id);

            entity.Id = id;
            entity.Edited = _systemClock.UtcNow.Date;

            UpdatePartitionKey(entity, pk);

            await _container.UpsertItemAsync(entity, GetPartitionKey(entity));
        }

        public override async Task RemoveAsync(string compoundId)
        {
            if (!(await ExistsAsync(compoundId)))
                throw new NotFoundException();

            if (_isSoftRemovable)
            {
                var entity = await GetAsync(compoundId);

                if (entity is ISoftRemovableEntity softRemovableEntity)
                {
                    softRemovableEntity.Removed = _systemClock.UtcNow.Date;

                    await _container.UpsertItemAsync(entity, GetPartitionKey(entity));

                    return;
                }
            }

            var (id, pk) = GetIdAndPartitionKey(compoundId);

            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(pk));
        }

        public override async Task<bool> ExistsAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKey(compoundId);

            var partitionKeyPropertyName = JsonNamingPolicy.CamelCase.ConvertName(GetPartitionKeyPropertyName());

            var query = new QueryDefinition($"SELECT c.id FROM c WHERE c.id = @id AND c.{partitionKeyPropertyName} = @pk")
                .WithParameter("@id", id)
                .WithParameter("@pk", pk);

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

        protected PartitionKey GetPartitionKey(TEntity entity)
            => new(GetPartitionKeyPropertyInfo()?.GetValue(entity) as string);

        protected void UpdatePartitionKey(TEntity entity, object value)
            => GetPartitionKeyPropertyInfo()?.SetValue(entity, value);

        protected (string id, string pk) GetIdAndPartitionKey(string compoundId)
        {
            if (!UsesCompoundId)
                return (compoundId, compoundId);

            return CosmosDbRepositoryUtility.GetIdAndPartitionKeyFromCompoundKey(compoundId);
        }

        private PropertyInfo? GetPartitionKeyPropertyInfo()
            => typeof(TEntity).GetProperty(GetPartitionKeyPropertyName());
    }
}

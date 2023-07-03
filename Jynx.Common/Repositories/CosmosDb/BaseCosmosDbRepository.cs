﻿using Jynx.Common.Abstractions.Chronometry;
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
        private readonly Container _container;
        private readonly IDateTimeService _dateTimeService;
        private readonly bool _isSoftRemovable;

        protected abstract CosmosDbContainerInfo ContainerInfo { get; }

        protected BaseCosmosDbRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            IDateTimeService dateTimeService,
            ILogger logger)
            : base(logger)
        {
            _isSoftRemovable = typeof(TEntity).IsAssignableTo(typeof(ISoftRemovableEntity));
            _container = cosmosClient.GetContainer(cosmosDbOptions.Value.DatabaseName, ContainerInfo.Name);
            _dateTimeService = dateTimeService;
        }

        protected virtual string GenerateId(TEntity entity)
            => Guid.NewGuid().ToString();

        protected virtual PartitionKey ResolvePartitionKey(TEntity entity)
            => new(entity.Id);

        public override async Task<string> CreateAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            entity.Created = _dateTimeService.UtcNow;

            try
            {
                var result = await _container.CreateItemAsync(entity, ResolvePartitionKey(entity));

                return result.Resource.Id!;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Conflict)
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
            catch (CosmosException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                throw;
            }
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            entity.Edited = _dateTimeService.UtcNow;

            await _container.UpsertItemAsync(entity, ResolvePartitionKey(entity));
        }

        public override async Task RemoveAsync(string compoundId)
        {
            if (_isSoftRemovable)
            {
                var entity = await ReadAsync(compoundId);

                if (entity is ISoftRemovableEntity softRemovableEntity)
                {
                    softRemovableEntity.Removed = _dateTimeService.UtcNow;

                    await _container.UpsertItemAsync(entity, ResolvePartitionKey(entity));

                    return;
                }
            }

            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(pk));
        }

        public override Task RemoveAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                throw new Exception("Can not delete an entity without an id");

            return RemoveAsync(entity.Id);
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

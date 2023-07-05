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
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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

        protected virtual string GenerateId(TEntity entity)
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        protected virtual string GetCompoundId(TEntity entity)
            => CreateCompoundId(GetPartitionKeyValue(entity), entity.Id!);

        protected virtual string GetPartitionKeyValue(TEntity entity)
            => CreatePartitionKeyHash(entity.Id!);

        public override async Task<string> CreateAsync(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
                entity.Id = GenerateId(entity);

            entity.Created = _systemClock.UtcNow.Date;

            try
            {
                var pk = GetPartitionKeyValue(entity);

                var cosmosEntity = entity.ToDynamic();
                cosmosEntity.pk = pk;

                var result = await _container.CreateItemAsync(cosmosEntity, new PartitionKey(pk));
                entity.Id = result.Resource.id;

                var compoundId = GetCompoundId(entity);

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
                var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

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

            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(entity.Id);

            entity.Id = id;
            entity.Edited = _systemClock.UtcNow.Date;

            await _container.UpsertItemAsync(entity, new PartitionKey(pk));
        }

        public override async Task RemoveAsync(string compoundId)
        {
            if (!(await ExistsAsync(compoundId)))
                throw new NotFoundException();

            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            if (_isSoftRemovable)
            {
                var entity = await GetAsync(compoundId);

                if (entity is ISoftRemovableEntity softRemovableEntity)
                {
                    softRemovableEntity.Removed = _systemClock.UtcNow.Date;

                    await _container.UpsertItemAsync(entity, new PartitionKey(pk));

                    return;
                }
            }

            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(pk));
        }

        public override async Task<bool> ExistsAsync(string compoundId)
        {
            var (id, pk) = GetIdAndPartitionKeyFromCompoundKey(compoundId);

            var query = new QueryDefinition($"SELECT c.id FROM c WHERE c.id = @id AND c.pk = @pk")
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

        protected static string CreateCompoundId(params string[] parts)
            => string.Join(".", parts);

        protected static (string id, string pk) GetIdAndPartitionKeyFromCompoundKey(string compoundId)
        {
            var parts = compoundId.Split(".");

            if (parts.Length != 2)
                throw new InvalidCompoundIdException();

            return (parts[1], parts[0]);
        }

        protected static string CreatePartitionKeyHash(params object[] parts)
        {
            using var sha1 = SHA1.Create();

            var bytes = Encoding.UTF8.GetBytes(string.Join('+', parts.Select(o => o.ToString())));

            var hashBytes = sha1.ComputeHash(bytes);

            var hash = WebEncoders.Base64UrlEncode(hashBytes);

            return hash;
        }
    }
}

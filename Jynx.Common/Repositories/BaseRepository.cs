﻿using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Repositories
{
    internal abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected BaseRepository(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public abstract Task<string> CreateAsync(TEntity entity);

        public abstract Task<TEntity?> GetAsync(string id);

        public abstract Task UpdateAsync(TEntity entity);

        public abstract Task RemoveAsync(string id);

        public abstract Task<bool> ExistsAsync(string id);
    }
}

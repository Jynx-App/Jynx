﻿using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Common.Repositories.CosmosDb
{
    internal class ApiAppUsersRepository : BaseCosmosDbRepository<ApiAppUser>, IApiAppUsersRepository
    {
        public ApiAppUsersRepository(
            CosmosClient cosmosClient,
            IOptions<CosmosDbOptions> cosmosDbOptions,
            ISystemClock systemClock,
            ILogger<ApiAppUsersRepository> logger)
            : base(cosmosClient, cosmosDbOptions, systemClock, logger)
        {
        }

        protected override CosmosDbContainerInfo ContainerInfo => new()
        {
            Name = "ApiAppUsers"
        };
    }
}

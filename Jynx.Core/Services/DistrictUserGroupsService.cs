﻿using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class DistrictUserGroupsService : RepositoryService<IDistrictUserGroupsRepository, DistrictUserGroup>, IDistrictUserGroupsService
    {
        public DistrictUserGroupsService(
            IDistrictUserGroupsRepository repository,
            IValidator<DistrictUserGroup> validator,
            ISystemClock systemClock,
            ILogger<DistrictUserGroupsService> logger)
            : base(repository, validator, systemClock, logger)
        {
        }
    }
}

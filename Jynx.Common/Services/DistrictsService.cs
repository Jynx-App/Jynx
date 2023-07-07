﻿using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Services;
using Jynx.Common.Entities.Validation;
using Jynx.Common.Repositories.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictsService : RepositoryService<IDistrictsRepository, District>, IDistrictsService
    {
        private readonly IDistrictUsersService _districtUsersService;
        private readonly IDistrictUserGroupsService _districtUserGroupsService;
        private readonly IUsersService _usersService;

        public string DefaultNotAllowedToPostMessage => "You are not allowed to post in this district";
        public string DefaultNotAllowedToCommentMessage => "You are not allowed to comment in this district";

        public DistrictsService(
            IDistrictsRepository districtRepository,
            IDistrictUsersService districtUsersService,
            IDistrictUserGroupsService districtUserGroupsService,
            IUsersService usersService,
            ISystemClock systemClock,
            ILogger<DistrictsService> logger)
            : base(districtRepository, new DistrictValidator(), systemClock, logger)
        {
            _districtUsersService = districtUsersService;
            _districtUserGroupsService = districtUserGroupsService;
            _usersService = usersService;
        }

        public async Task<string> CreateAndAssignModerator(District district, string userId)
        {
            var userExists = await _usersService.ExistsAsync(userId);

            if (!userExists)
                throw new NotFoundException(nameof(User));

            district.Id = await Repository.CreateAsync(district);

            var districtUser = new DistrictUser
            {
                Id = userId,
                DistrictId = district.Id,
                ModerationPermissions = Enum.GetValues<ModerationPermission>().ToHashSet()
            };

            districtUser.Id = await _districtUsersService.CreateAsync(districtUser);

            return district.Id;
        }

        public async Task<bool> IsUserAllowedToPostAndCommentAsync(string districtId, string userId)
        {
            var district = await GetAsync(districtId);

            if (district is null)
                return false;

            var districtUser = await _districtUsersService.GetByDistrictIdAndUserId(districtId, userId);

            if (districtUser is null)
                return true;

            if (districtUser.BannedUntil <= SystemClock.UtcNow)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a User has a permission in either a related DistrictUser or DistrictUserGroup
        /// </summary>
        /// <param name="districtId"></param>
        /// <param name="userId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public async Task<bool> DoesUserHavePermissionAsync(string districtId, string userId, ModerationPermission permission)
        {
            var districtUser = await _districtUsersService.GetByDistrictIdAndUserId(districtId, userId);

            if (districtUser is null)
                return false;

            if (districtUser.ModerationPermissions.Contains(permission))
                return true;

            if (!string.IsNullOrWhiteSpace(districtUser.DistrictUserGroupId))
            {
                var districtUserGroup = await _districtUserGroupsService.GetAsync(districtUser.DistrictUserGroupId);

                if (districtUserGroup is not null && districtUserGroup.ModerationPermissions.Contains(permission))
                    return true;
            }

            return false;
        }
    }
}

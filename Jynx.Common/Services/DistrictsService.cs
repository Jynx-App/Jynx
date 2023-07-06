using FluentValidation;
using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Auth;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class DistrictsService : RepositoryService<IDistrictsRepository, District>, IDistrictsService
    {
        private readonly IDistrictUsersService _districtUsersService;
        private readonly IDistrictUserGroupsService _districtUserGroupsService;

        public string DefaultNotAllowedToPostMessage => "You are not allowed to post in this district";
        public string DefaultNotAllowedToCommentMessage => "You are not allowed to comment in this district";

        public DistrictsService(
            IDistrictsRepository districtRepository,
            IDistrictUsersService districtUsersService,
            IDistrictUserGroupsService districtUserGroupsService,
            IValidator<District> validator,
            ISystemClock systemClock,
            ILogger<DistrictsService> logger)
            : base(districtRepository, validator, systemClock, logger)
        {
            _districtUsersService = districtUsersService;
            _districtUserGroupsService = districtUserGroupsService;
        }

        public async Task<bool> IsUserAllowedToPostAndCommentAsync(string districtId, string userId)
        {
            var district = await GetAsync(districtId);

            if (district is null)
                return false;

            //Todo: Logic for preventing users from posting to districts they're banned/suspended from goes here

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

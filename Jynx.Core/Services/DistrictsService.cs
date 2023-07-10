using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Jynx.Abstractions.Services;
using Jynx.Common.Events;
using Jynx.Core.Services.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace Jynx.Core.Services
{
    internal class DistrictsService : RepositoryService<IDistrictsRepository, District>, IDistrictsService, IEventSubscriber<CreatePostEvent>
    {
        private readonly IDistrictUsersService _districtUsersService;
        private readonly IDistrictUserGroupsService _districtUserGroupsService;
        private readonly IPostsService _postsService;

        public string DefaultNotAllowedToPostMessage => "You are not allowed to post in this district";
        public string DefaultNotAllowedToCommentMessage => "You are not allowed to comment in this district";

        public DistrictsService(
            IDistrictsRepository districtRepository,
            IDistrictUsersService districtUsersService,
            IDistrictUserGroupsService districtUserGroupsService,
            IPostsService postsService,
            IValidator<District> validator,
            ISystemClock systemClock,
            ILogger<DistrictsService> logger)
            : base(districtRepository, validator, systemClock, logger)
        {
            _districtUsersService = districtUsersService;
            _districtUserGroupsService = districtUserGroupsService;
            _postsService = postsService;
        }

        public async Task<string> CreateAndAssignModerator(District district, string userId)
        {
            district.Id = await CreateAsync(district);

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

        public async Task<IEnumerable<Post>> GetPostsAsync(string districtId, int count, int offset = 0, PostsSortOrder? sortOrder = null)
        {
            var district = await GetAsync(districtId) ?? throw new NotFoundException(nameof(District));

            sortOrder ??= district.DefaultPostSortOrder;

            var posts = await _postsService.GetByDistrictIdAsync(districtId, offset, count, sortOrder.Value);

            return posts;
        }

        async Task IEventSubscriber<CreatePostEvent>.HandleAsync(object seender, CreatePostEvent ev)
        {
            var district = await GetAsync(ev.Post.DistrictId) ?? throw new NotFoundException(nameof(District));

            ev.Post.DefaultCommentsSortOrder = district.DefaultCommentsSortOrder;
        }
    }
}

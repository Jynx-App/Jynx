﻿using FluentValidation;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Repositories;
using Jynx.Abstractions.Repositories.Exceptions;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Services.Exceptions;
using Jynx.Common.Events;
using Jynx.Core.Configuration;
using Jynx.Core.Services.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jynx.Core.Services
{
    internal class DistrictsService : RepositoryService<IDistrictsRepository, District>,
        IDistrictsService,
        IEventSubscriber<CreatingPostEvent>,
        IEventSubscriber<PinningPostEvent>,
        IEventSubscriber<PinningCommentEvent>
    {
        private readonly IDistrictUsersService _districtUsersService;
        private readonly IDistrictUserGroupsService _districtUserGroupsService;
        private readonly IPostsService _postsService;
        private readonly IOptions<DistrictsOptions> _districtOptions;

        public DistrictsService(
            IDistrictsRepository districtRepository,
            IDistrictUsersService districtUsersService,
            IDistrictUserGroupsService districtUserGroupsService,
            IPostsService postsService,
            IValidator<District> validator,
            ISystemClock systemClock,
            IOptions<DistrictsOptions> districtOptions,
            ILogger<DistrictsService> logger)
            : base(districtRepository, validator, systemClock, logger)
        {
            _districtUsersService = districtUsersService;
            _districtUserGroupsService = districtUserGroupsService;
            _postsService = postsService;
            _districtOptions = districtOptions;
        }

        public async Task<District> CreateAndAssignModerator(District district, string userId)
        {
            district = await CreateAsync(district);

            var districtUser = new DistrictUser
            {
                Id = userId,
                DistrictId = district.Id!,
                ModerationPermissions = Enum.GetValues<ModerationPermission>().ToHashSet()
            };

            districtUser = await _districtUsersService.CreateAsync(districtUser);

            return district;
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

            var posts = (await _postsService.GetPinnedByDistrictIdAsync(district.Id!)).ToList();
            posts.AddRange(await _postsService.GetByDistrictIdAsync(district.Id!, offset, count - posts.Count, sortOrder.Value));

            return posts;
        }

        async Task IEventSubscriber<CreatingPostEvent>.HandleAsync(object sender, CreatingPostEvent @event)
        {
            var district = await GetAsync(@event.Post.DistrictId) ?? throw new NotFoundException(nameof(District));

            @event.Post.DefaultCommentsSortOrder = district.DefaultCommentsSortOrder;
        }

        async Task IEventSubscriber<PinningPostEvent>.HandleAsync(object sender, PinningPostEvent @event)
        {
            if (!@event.Pin)
                return;

            var district = await GetAsync(@event.Post.DistrictId) ?? throw new NotFoundException(nameof(District));

            var maxPinnedPosts = _districtOptions.Value.MaxPinnedPosts;

            if (@event.NumberOfCurrentlyPinnedPosts >= maxPinnedPosts)
                throw new PinnedLimitException(maxPinnedPosts, nameof(Post));
        }

        async Task IEventSubscriber<PinningCommentEvent>.HandleAsync(object sender, PinningCommentEvent @event)
        {
            if (!@event.Pin)
                return;

            var district = await GetAsync(@event.Comment.DistrictId) ?? throw new NotFoundException(nameof(District));

            var maxPinnedComments = _districtOptions.Value.MaxPinnedComments;

            if (@event.NumberOfCurrentlyPinnedComments >= maxPinnedComments)
                throw new PinnedLimitException(maxPinnedComments, nameof(Comment));
        }
    }
}

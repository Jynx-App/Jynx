﻿using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Common.Configuration;
using Jynx.Core.Configuration;
using Jynx.Core.Services;
using Jynx.Validation.Fluent;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Core
{
    public static class CoreExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services
                // Configuration
                .ConfigureByDefaultKey<OfficialApiAppOptions>(configuration)
                .ConfigureByDefaultKey<DistrictsOptions>(configuration)
                .ConfigureByDefaultKey<PostsOptions>(configuration)
                .ConfigureByDefaultKey<CommentsOptions>(configuration)
                .ConfigureByDefaultKey<UsersOptions>(configuration)
                // Services
                .AddScoped<IApiAppsService, ApiAppService>()
                .AddScoped<IApiAppUsersService, ApiAppUsersService>()
                .AddScoped<ICommentsService, CommentsService>()
                .AddScoped<ICommentVotesService, CommentVotesService>()
                .AddScoped<IDistrictsService, DistrictsService>()
                .AddScoped<IDistrictUsersService, DistrictUsersService>()
                .AddScoped<IDistrictUserGroupsService, DistrictUserGroupsService>()
                .AddScoped<INotificationsService, NotificationsService>()
                .AddScoped<IPostsService, PostsService>()
                .AddScoped<IPostVotesService, PostVotesService>()
                .AddScoped<IUsersService, UsersService>()
                // Other
                .AddFluentValidators()
                .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
                .AddHttpContextAccessor();

            return services;
        }
    }
}

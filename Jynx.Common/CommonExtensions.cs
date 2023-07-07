using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Common.Auth;
using Jynx.Common.Configuration;
using Jynx.Common.Entities.Validation;
using Jynx.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common
{
    public static class CommonExtensions
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services
                // Configuration
                .ConfigureByDefaultKey<OfficialApiAppOptions>(configuration)
                // Services
                .AddScoped<IApiAppsService, ApiAppService>()
                .AddScoped<IApiAppUsersService, ApiAppUsersService>()
                .AddScoped<ICommentsService, CommentsService>()
                .AddScoped<IDistrictsService, DistrictsService>()
                .AddScoped<IDistrictUsersService, DistrictUsersService>()
                .AddScoped<IDistrictUserGroupsService, DistrictUserGroupsService>()
                .AddScoped<INotificationsService, NotificationsService>()
                .AddScoped<IPostsService, PostsService>()
                .AddScoped<IPostVotesService, PostVotesService>()
                .AddScoped<IUsersService, UsersService>()
                // PolicyProviders
                .AddSingleton<IAuthorizationPolicyProvider, RequireModerationPermissionPolicyProvider>()
                // Other
                .AddEntityValidators()
                .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
                .AddHttpContextAccessor()
                .AddScoped<IAuthorizationHandler, RequireModerationPermissionHandler>();

            return services;
        }
    }
}

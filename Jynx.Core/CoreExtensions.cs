using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Common.Configuration;
using Jynx.Common.DependencyInjection;
using Jynx.Common.Events;
using Jynx.Core.Configuration;
using Jynx.Core.Services;
using Jynx.Core.Services.Events;
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
                .AddScoped<DistrictsService>()
                    .ForwardScoped<IDistrictsService, DistrictsService>()
                    .ForwardScoped<IEventSubscriber<CreatingPostEvent>, DistrictsService>()
                    .ForwardScoped<IEventSubscriber<PinningPostEvent>, DistrictsService>()
                    .ForwardScoped<IEventSubscriber<PinningCommentEvent>, DistrictsService>()
                .AddScoped<DistrictsService>()
                .AddScoped<IDistrictsService>(sp => sp.GetService<DistrictsService>()!)
                .AddScoped<IEventSubscriber<CreatingPostEvent>>(sp => sp.GetService<DistrictsService>()!)
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

using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Azure.CosmosDb;
using Jynx.Common.Repositories.CosmosDb;
using Jynx.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common
{
    public static class CommonExtensions
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services
                // Repositories
                .AddScoped<IApiAppsRepository, ApiAppsRepository>()
                .AddScoped<IApiAppUsersRepository, ApiAppUsersRepository>()
                .AddScoped<ICommentsRepository, CommentsRepository>()
                .AddScoped<IDistrictsRepository, DistrictsRepository>()
                .AddScoped<IDistrictUsersRepository, DistrictUsersRepository>()
                .AddScoped<IDistrictUserGroupsRepository, DistrictUserGroupsRepository>()
                .AddScoped<INotificationsRepository, NotificationsRepository>()
                .AddScoped<IPostsRepository, PostsRepository>()
                .AddScoped<IUsersRepository, UsersRepository>()
                // Services
                .AddScoped<IApiAppService, ApiAppService>()
                .AddScoped<IApiAppUsersService, ApiAppUsersService>()
                .AddScoped<ICommentsService, CommentsService>()
                .AddScoped<IDistrictsService, DistrictsService>()
                .AddScoped<IDistrictUsersService, DistrictUsersService>()
                .AddScoped<IDistrictUserGroupsService, DistrictUserGroupsService>()
                .AddScoped<INotificationsService, NotificationsService>()
                .AddScoped<IPostsService, PostsService>()
                .AddScoped<IUsersService, UsersService>()
                // Other
                .AddCosmosDb(configuration);

            return services;
        }
    }
}

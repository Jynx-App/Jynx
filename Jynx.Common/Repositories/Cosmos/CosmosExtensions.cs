using Jynx.Common.Abstractions.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Repositories.Cosmos
{
    internal static class CosmosExtensions
    {
        public static IServiceCollection AddCosmosRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IApiAppsRepository, ApiAppsRepository>()
                .AddScoped<IApiAppUsersRepository, ApiAppUsersRepository>()
                .AddScoped<ICommentsRepository, CommentsRepository>()
                .AddScoped<IDistrictsRepository, DistrictsRepository>()
                .AddScoped<IDistrictUsersRepository, DistrictUsersRepository>()
                .AddScoped<IDistrictUserGroupsRepository, DistrictUserGroupsRepository>()
                .AddScoped<INotificationsRepository, NotificationsRepository>()
                .AddScoped<IPostsRepository, PostsRepository>()
                .AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}

using Jynx.Abstractions.Repositories;
using Jynx.Common.Configuration;
using Jynx.Common.DependencyInjection;
using Jynx.Data.Cosmos.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Data.Cosmos
{
    public static class CosmosExtensions
    {
        public static IServiceCollection AddCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCosmosClient(configuration)
                .AddCosmosRepositories();

            return services;
        }

        public static IServiceCollection AddCosmosClient(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureByDefaultKey<CosmosOptions>(configuration)
                .AddSingleton(sp =>
                {
                    var options = sp.GetIOptions<CosmosOptions>().Value;

                    var cosmosClient = new CosmosClient(options.Endpoint, options.PrimaryKey, new()
                    {
                        SerializerOptions = new CosmosSerializationOptions
                        {
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                            IgnoreNullValues = true
                        }
                    });

                    return cosmosClient;
                });

            return services;
        }

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
                .AddScoped<IPostVotesRepository, PostVoteRepository>()
                .AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}

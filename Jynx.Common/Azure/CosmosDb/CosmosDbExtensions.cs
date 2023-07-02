using Jynx.Common.Configuration;
using Jynx.Common.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Azure.CosmosDb
{
    internal static class CosmosDbExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureByDefaultKey<CosmosDbOptions>(configuration)
                .AddSingleton(sp =>
                {
                    var options = sp.GetIOptions<CosmosDbOptions>().Value;

                    return new CosmosClient(options.Endpoint, options.PrimaryKey);
                });

            return services;
        }
    }
}

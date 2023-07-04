using Jynx.Common.Configuration;
using Jynx.Common.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Azure.Cosmos
{
    internal static class CosmosExtensions
    {
        public static IServiceCollection AddCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureByDefaultKey<CosmosOptions>(configuration)
                .AddSingleton(sp =>
                {
                    var options = sp.GetIOptions<CosmosOptions>().Value;

                    return new CosmosClient(options.Endpoint, options.PrimaryKey);
                });

            return services;
        }
    }
}

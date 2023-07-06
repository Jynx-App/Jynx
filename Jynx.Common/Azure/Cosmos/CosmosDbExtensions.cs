using Jynx.Common.Configuration;
using Jynx.Common.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Jynx.Common.Azure.Cosmos
{
    internal static class CosmosExtensions
    {
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
    }
}

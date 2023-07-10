using Jynx.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common
{
    public static class CommonExtensions
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEventPublisher();

            return services;
        }
    }
}

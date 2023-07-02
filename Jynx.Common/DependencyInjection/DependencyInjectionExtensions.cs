using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jynx.Common.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static HttpClient? GetHttpClient<TName>(this IServiceProvider services)
            => services.GetHttpClient(typeof(TName).Name);

        public static HttpClient? GetHttpClient(this IServiceProvider services, string clientName)
            => services.GetService<IHttpClientFactory>()?.CreateClient(clientName);

        public static IOptions<TOption> GetIOptions<TOption>(this IServiceProvider services)
            where TOption : class
            => services.GetService<IOptions<TOption>>() ?? throw new Exception($"Could not pull IOptions<{typeof(TOption).Name}> out of DI");
    }
}

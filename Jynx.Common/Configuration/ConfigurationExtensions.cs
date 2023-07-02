using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Jynx.Common.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T? GetByDefaultKey<T>(this IConfiguration configuration)
            => configuration.GetSectionUsingDefaultKey<T>().Get<T>();

        public static IConfiguration GetSectionUsingDefaultKey<T>(this IConfiguration configuration)
            => configuration.GetSection(GetDefaultKeyValue<T>());

        public static IServiceCollection ConfigureByDefaultKey<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class
        {
            services.Configure<T>(configuration.GetSectionUsingDefaultKey<T>());

            return services;
        }

        private static string GetDefaultKeyValue<T>()
        {
            var type = typeof(T);

            var defaultKeyField = type.GetField("DefaultKey", BindingFlags.Public | BindingFlags.Static);

            var defaultKey = defaultKeyField?.GetValue(null) as string;

            if (string.IsNullOrWhiteSpace(defaultKey))
                throw new Exception($"{type.FullName} does not have a const field of type string called 'DefaultKey' or it is empty");

            return defaultKey;
        }
    }
}

using Microsoft.AspNetCore.Builder;

namespace Jynx.Common.Logging
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder AddJynxLogging(this IApplicationBuilder app, Action<Dictionary<string, object>> stateHandler)
        {
            app.UseMiddleware<JynxLoggingMiddleware>(stateHandler);

            return app;
        }
    }
}

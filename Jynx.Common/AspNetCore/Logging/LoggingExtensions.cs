using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Jynx.Common.AspNetCore.Logging
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder AddJynxLogging(this IApplicationBuilder app, Action<HttpContext, Dictionary<string, object>> stateHandler)
        {
            app.UseMiddleware<JynxLoggingMiddleware>(stateHandler);

            return app;
        }
    }
}

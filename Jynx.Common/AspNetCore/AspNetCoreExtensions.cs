using Microsoft.AspNetCore.Builder;

namespace Jynx.Common.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IApplicationBuilder EnableRequestBodyBuffering(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestBodyBufferingMiddleware>();

            return app;
        }
    }
}

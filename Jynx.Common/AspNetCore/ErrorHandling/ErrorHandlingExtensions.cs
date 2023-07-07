using Microsoft.AspNetCore.Builder;

namespace Jynx.Common.AspNetCore.ErrorHandling
{
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder AddJynxErrorHandling(this IApplicationBuilder app, Func<Exception, bool>? exceptionHandler = null)
        {
            app.UseMiddleware<JynxErrorHandlingMiddleware>(exceptionHandler);

            return app;
        }
    }
}

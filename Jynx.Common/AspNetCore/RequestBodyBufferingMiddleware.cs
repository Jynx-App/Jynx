using Microsoft.AspNetCore.Http;

namespace Jynx.Common.AspNetCore
{
    internal class RequestBodyBufferingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestBodyBufferingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
            }
            catch
            {
                // Do nothing...
            }

            await _next(context);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Jynx.Common.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static string GetRequestId(this HttpContext context)
            => Activity.Current?.Id ?? context.TraceIdentifier;
    }
}

using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Jynx.Common.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        private const string _xRequestedWithHeaderName = "X-Requested-With";
        private const string _acceptHeaderName = "Accept";
        private const string _xRequestedWithAjaxValue = "XMLHttpRequest";
        private const string _authorizationHeaderName = "Authorization";

        public static async Task<string> GetBodyAsStringAsync(this HttpRequest request)
        {
            request.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(request.Body, leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            request.Body.Seek(0, SeekOrigin.Begin);

            return body;
        }

        public static string? GetAuthenticationValue(this HttpRequest request)
        {
            if (!request.Headers.ContainsKey(_authorizationHeaderName))
                return null;

            var value = request.Headers[_authorizationHeaderName].ToString();

            if (value.StartsWith("Bearer "))
                return value[7..];

            return value;
        }

        public static string GetIp(this HttpRequest request)
            => request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? throw new Exception();

        public static bool IsAjax(this HttpRequest request)
            => request.Headers.ContainsKey(_xRequestedWithHeaderName) && request.Headers[_xRequestedWithHeaderName] == _xRequestedWithAjaxValue;

        public static bool IsJson(this HttpRequest request)
            => request.Headers.ContainsKey(_acceptHeaderName) && request.Headers[_acceptHeaderName].ToString().Split(',').Any(t => t.Equals(Application.Json, StringComparison.OrdinalIgnoreCase));
    }
}

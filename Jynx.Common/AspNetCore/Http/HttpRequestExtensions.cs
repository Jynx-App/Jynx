using Microsoft.AspNetCore.Http;

namespace Jynx.Common.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        private const string _authorizationHeaderName = "Authorization";

        public static async Task<string> GetBodyAsStringAsync(this HttpRequest request)
        {
            var originalPosition = request.Body.Position;

            request.Body.Seek(0, SeekOrigin.Begin);

            using var reader = new StreamReader(request.Body);

            var body = await reader.ReadToEndAsync();

            request.Body.Position = originalPosition;

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
    }
}

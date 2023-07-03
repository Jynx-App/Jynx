using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Jynx.Common.Web.Http
{
    public static class HttpRequestExtensions
    {
        private const string _xForwardForHeaderName = "X-Forwarded-For";
        private const string _xRequestedWithHeaderName = "X-Requested-With";
        private const string _acceptHeaderName = "Accept";
        private const string _xRequestedWithAjaxValue = "XMLHttpRequest";

        public static string GetIp(this HttpRequest request)
        {
            if (!request.Headers.ContainsKey(_xForwardForHeaderName))
                return request.HttpContext.Connection.RemoteIpAddress.ToString();

            return request.Headers[_xForwardForHeaderName].ToString().Split(':')[0];
        }

        public static bool IsAjax(this HttpRequest request)
            => request.Headers.ContainsKey(_xRequestedWithHeaderName) && request.Headers[_xRequestedWithHeaderName] == _xRequestedWithAjaxValue;

        public static bool IsJson(this HttpRequest request)
            => request.Headers.ContainsKey(_acceptHeaderName) && request.Headers[_acceptHeaderName].ToString().Split(',').Any(t => t.Equals(Application.Json, StringComparison.OrdinalIgnoreCase));
    }
}

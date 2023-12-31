﻿using Jynx.Common.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.AspNetCore.Logging
{
    public class JynxLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly Action<HttpContext, Dictionary<string, object?>> _stateHandler;

        public JynxLoggingMiddleware(
            RequestDelegate next,
            Action<HttpContext, Dictionary<string, object?>> stateHandler,
            ILogger<JynxLoggingMiddleware> logger)
        {
            _next = next;
            _stateHandler = stateHandler;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var state = new Dictionary<string, object?>
            {
                ["Ip"] = context.Request.GetIp(),
            };

            _stateHandler(context, state);

            try
            {
                using (_logger.BeginScope(state))
                {
                    await _next(context);
                }
            }
            catch (Exception ex) when (LogException(ex))
            {
                // Never gets executed due to when clause.
            }
        }

        private bool LogException(Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred while executing the request.");
            return false;
        }
    }
}

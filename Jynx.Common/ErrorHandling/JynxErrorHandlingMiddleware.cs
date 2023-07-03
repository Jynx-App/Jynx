using Jynx.Common.AspNetCore.Http;
using Jynx.Common.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Jynx.Common.ErrorHandling
{
    public class JynxErrorHandlingMiddleware
    {
        private const string _exceptionLogMessage = "An unhandled exception has occurred while executing the request.";
        private const string _defaultSafeMessage = "An Unhandled Error has occured";
        private const string _genericErrorViewName = "Errors/GenericError";

        private readonly RequestDelegate _next;
        private readonly Func<Exception, bool> _exceptionHandler;
        private readonly ILogger<JynxErrorHandlingMiddleware> _logger;

        public JynxErrorHandlingMiddleware(
            RequestDelegate next,
            Func<Exception, bool> exceptionHandler,
            ILogger<JynxErrorHandlingMiddleware> logger)
        {
            _next = next;
            _exceptionHandler = exceptionHandler;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var jex = ex as JynxException;

                var severity = jex?.Severity ?? JynxExceptionSeverity.Normal;

                switch (severity)
                {
                    case JynxExceptionSeverity.Minor:
                        _logger.LogDebug(ex, _exceptionLogMessage);
                        break;
                    case JynxExceptionSeverity.Critical:
                        _logger?.LogCritical(ex, _exceptionLogMessage);
                        break;
                    default:
                        _logger.LogError(ex, _exceptionLogMessage);
                        break;
                }

                if (_exceptionHandler?.Invoke(ex) == true)
                    return;

                var model = new GenericErrorModel
                {
                    Message = jex?.SafeMessage ?? _defaultSafeMessage,
                    RequestId = context.GetRequestId()
                };

                await RenderAsync(model, context, jex?.StatusCode ?? HttpStatusCode.BadRequest);
            };
        }

        private static async Task RenderAsync(object model, HttpContext context, HttpStatusCode statusCode)
        {
            var executor = context.RequestServices.GetService<IActionResultExecutor<ViewResult>>();

            context.Response.StatusCode = (int)statusCode;

            if (executor is null)
            {
                var json = JsonConvert.SerializeObject(model);

                await context.Response.WriteAsync(json);

                return;
            }

            var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var view = new ViewResult
            {
                ViewName = _genericErrorViewName,
                ViewData = viewData
            };

            await executor.ExecuteAsync(actionContext, view);
        }
    }
}

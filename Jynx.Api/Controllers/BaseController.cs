using Jynx.Api.Models.Responses;
using Jynx.Common.AspNetCore.Http;
using Jynx.Common.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public BaseController(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public IActionResult NotFound(string message)
            => NotFound(new GenericErrorModel
            {
                Message = message,
                RequestId = Request.HttpContext.GetRequestId()
            });

        public IActionResult BadRequest(string message)
            => BadRequest(new GenericErrorModel
            {
                Message = message,
                RequestId = Request.HttpContext.GetRequestId()
            });

        public IActionResult MultiError(IEnumerable<string> errors)
            => BadRequest(new MultiErrorResponse
            {
                Errors = errors,
                RequestId = Request.HttpContext.GetRequestId()
            });

        public IActionResult ModelStateError()
            => MultiError(ModelState.Select(ms => $"{ms.Key}={ms.Value}"));
    }
}

using Jynx.Common.AspNetCore.Http;
using Jynx.Common.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas
{
    [Route("[area]/[controller]/[action]")]
    public abstract class BaseAreaController : ControllerBase
    {
        public BaseAreaController(ILogger logger)
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
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas
{
    [Route("[area]/[controller]/[action]")]
    public abstract class BaseAreaController : ControllerBase
    {
        private readonly ILogger _logger;

        public BaseAreaController(ILogger logger)
        {
            _logger = logger;
        }
    }
}

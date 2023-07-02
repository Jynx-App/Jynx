using Microsoft.AspNetCore.Mvc;

namespace Jynx.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
    }
}

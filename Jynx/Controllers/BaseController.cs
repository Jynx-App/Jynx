using Microsoft.AspNetCore.Mvc;

namespace Jynx.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected readonly ILogger Logger;
    }
}

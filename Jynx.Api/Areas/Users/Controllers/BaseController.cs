using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Users.Controllers
{
    [Area("Users")]
    public abstract class BaseController : BaseAreaController
    {
        protected BaseController(ILogger logger)
            : base(logger)
        {
        }
    }
}

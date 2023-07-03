using Jynx.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas
{
    [Route("[area]/[controller]/[action]")]
    public abstract class BaseAreaController : BaseController
    {
        protected BaseAreaController(ILogger logger)
            : base(logger)
        {
        }
    }
}

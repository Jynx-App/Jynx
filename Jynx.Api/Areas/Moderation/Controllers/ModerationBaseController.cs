using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Moderation.Controllers
{
    [Area("Moderation")]
    public class ModerationBaseController : BaseAreaController
    {
        public ModerationBaseController(ILogger logger)
            : base(logger)
        {
        }
    }
}

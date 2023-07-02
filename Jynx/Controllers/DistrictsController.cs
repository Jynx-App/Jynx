using Microsoft.AspNetCore.Mvc;

namespace Jynx.Controllers
{
    public class DistrictsController : BaseController
    {
        public DistrictsController(ILogger<DistrictsController> logger)
            : base(logger)
        {

        }

        [HttpGet("d/{id}")]
        public async Task<IActionResult> Read(string id)
        {
            return View();
        }
    }
}

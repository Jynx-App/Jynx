using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Users.Controllers
{
    public class PostsController : BaseController
    {
        public PostsController(ILogger logger) : base(logger)
        {
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Create()
        {
            throw new Exception();
        }

        [HttpGet()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Read(string id)
        {
            throw new Exception();
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Update()
        {
            throw new Exception();
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Delete()
        {
            throw new Exception();
        }
    }
}

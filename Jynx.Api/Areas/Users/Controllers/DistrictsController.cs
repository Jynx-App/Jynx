using Jynx.Common.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Users.Controllers
{
    public class DistrictsController : BaseController
    {
        private readonly IDistrictsService _districtsService;

        public DistrictsController(
            IDistrictsService districtsService,
            ILogger<DistrictsController> logger)
            : base(logger)
        {
            _districtsService = districtsService;
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> GetPosts(string id)
        {
            var district = await _districtsService.ReadAsync(id);

            Logger.LogInformation("Test");

            if (district is null)
                return NotFound("District not found");

            return Ok(district);
        }
    }
}

using Jynx.Common.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> GetPosts(string id)
        {
            var district = await _districtsService.ReadAsync(id);

            if (district is null)
                return NotFound("District not found");

            return Ok(district);
        }
    }
}

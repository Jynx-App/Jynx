using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistrictsController : BaseController
    {
        private readonly IDistrictsService _districtsService;
        private readonly IDistrictUsersService _districtUsersService;

        public DistrictsController(
            IDistrictsService districtsService,
            IDistrictUsersService districtUsersService,
            ILogger<DistrictsController> logger)
            : base(logger)
        {
            _districtsService = districtsService;
            _districtUsersService = districtUsersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDistrictRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var district = request.ToEntity();

            _ = await _districtsService.CreateAsync(district);

            var districtUser = new DistrictUser
            {
                Id = userId,
                DistrictId = district.Id!,
                ModerationPermissions = Enum.GetValues<ModerationPermission>().ToHashSet()
            };

            var id = await _districtUsersService.CreateAsync(districtUser);

            var newEntityUrl = Url.ActionLink(nameof(Get), null, new { id })!;

            return Created(newEntityUrl, $"\"{id}\"");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _districtsService.GetAsync(id);

            if (entity is null)
                return NotFound(_districtsService.DefaultNotFoundMessage);

            var response = new GetDistrictResponse(entity);

            return Ok(response);
        }
    }
}

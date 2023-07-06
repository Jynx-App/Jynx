﻿using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Auth;
using Jynx.Common.Entities;
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

            districtUser.Id = await _districtUsersService.CreateAsync(districtUser);

            return Ok($"\"{district.Id}\"");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _districtsService.GetAsync(id);

            if (entity is null)
                return NotFound(_districtsService.DefaultNotFoundMessage);

            var response = new ReadDistrictResponse(entity);

            return Ok(response);
        }
    }
}

using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Security.Claims;
using Jynx.Common.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Jynx.Api.Auth
{
    public class RequireModerationPermissionHandler : AuthorizationHandler<RequireModerationPermissionRequirement>
    {
        private const string _routeIdKeyName = "id";
        private readonly string[] _httpMethodsThatUseModel = new[]
        {
            "post",
            "put",
            "delete",
        };


        private readonly IDistrictsService _districtsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequireModerationPermissionHandler(
            IDistrictsService districtsService,
            IHttpContextAccessor httpContextAccessor)
        {
            _districtsService = districtsService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireModerationPermissionRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
                return;

            if (context.User is null)
                return;

            var httpMethod = httpContext.Request.Method.ToLower();

            var id = httpContext.GetRouteData().Values.ContainsKey(_routeIdKeyName)
                ? httpContext.GetRouteData().Values[_routeIdKeyName]?.ToString()
                : _httpMethodsThatUseModel.Contains(httpMethod)
                    ? await GetIdFromRequestAsync(httpContext)
                    : null;

            var districtId = GetDistrictIdFromId(id);

            if (string.IsNullOrWhiteSpace(districtId))
                return;

            var userId = context.User.GetId()!;

            var hasPermission = await _districtsService.DoesUserHavePermissionAsync(districtId, userId, requirement.Permission);

            if (hasPermission)
                context.Succeed(requirement);

            return;
        }

        private async Task<string?> GetIdFromRequestAsync(HttpContext context)
        {
            var json = await context.Request.GetBodyAsStringAsync();

            var model = JsonConvert.DeserializeObject<RequireModerationPermissionModel>(json);

            var districtId = model?.Id;

            return districtId;
        }

        private string? GetDistrictIdFromId(string? districtId)
            => districtId?.Split('.').FirstOrDefault() ?? districtId;

        private class RequireModerationPermissionModel
        {
            public string? Id { get; set; }

            public string? DistrictId { get; set; }
        }
    }

    public class RequireModerationPermissionRequirement : IAuthorizationRequirement
    {
        public RequireModerationPermissionRequirement(ModerationPermission requiredPermission)
        {
            Permission = requiredPermission;
        }

        public ModerationPermission Permission { get; }
    }
}

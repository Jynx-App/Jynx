using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
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

        public RequireModerationPermissionHandler(
            IDistrictsService districtsService)
        {
            _districtsService = districtsService;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireModerationPermissionRequirement requirement)
        {
            if (context.Resource is not HttpContext httpContext)
                return;

            if (context.User is null)
                return;

            var httpMethod = httpContext.Request.Method.ToLower();

            var districtId = httpContext.GetRouteData().Values.ContainsKey(_routeIdKeyName)
                ? httpContext.GetRouteData().Values[_routeIdKeyName]?.ToString()
                : _httpMethodsThatUseModel.Contains(httpMethod)
                    ? await GetDistrictIdFromRequestAsync(httpContext)
                    : null;

            if (string.IsNullOrWhiteSpace(districtId))
                return;

            var userId = context.User.GetId()!;

            var hasPermission = await _districtsService.DoesUserHavePermissionAsync(districtId, userId, requirement.Permission);

            if (hasPermission)
                context.Succeed(requirement);

            return;
        }

        private async Task<string?> GetDistrictIdFromRequestAsync(HttpContext context)
        {
            var json = await context.Request.GetBodyAsStringAsync();

            var model = JsonConvert.DeserializeObject<DistrictRelatedIdRequest>(json);

            return model?.DistrictId;
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

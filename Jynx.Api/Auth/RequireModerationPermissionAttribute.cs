using Jynx.Abstractions.Entities.Auth;
using BaseAuthorizationAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Jynx.Api.Auth
{
    public class RequireModerationPermissionAttribute : BaseAuthorizationAttribute
    {
        private const string _policyPrefix = "RequireModerationPermission";

        public RequireModerationPermissionAttribute(ModerationPermission permission)
            : base()
        {
            Permission = permission;

            Policy = $"{_policyPrefix}{permission}";
        }

        public ModerationPermission Permission { get; }
    }
}

using BaseAuthorizationAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Jynx.Common.Auth
{
    public class AuthorizeAttribute : BaseAuthorizationAttribute
    {
        public AuthorizeAttribute() { }

        public AuthorizeAttribute(string policy) : base(policy) { }

        public AuthorizeAttribute(params ModerationPermissions[] requiredPermissions)
        {
            Roles = string.Join(",", requiredPermissions.Select(p => p.ToString()));
        }
    }
}

using Aspor.Authorization.Extensions;
using Aspor.Authorization.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Aspor.Authorization.Attributes
{
    public class ScopeRequirement : Attribute, IAuthorizationFilter
    {

        public static string SCOPE_PREFIX = null;

        private readonly string[] _scopes;
        private readonly UserType _type;

        public ScopeRequirement(UserType type, string[] scopes)
        {
            _type = type;
            _scopes = scopes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AsporUser user = context.HttpContext.GetUser();
            if (user.Type == _type)
            {
                foreach (string scope in _scopes)
                {
                    if (user.Scopes.Contains(SCOPE_PREFIX != null ? SCOPE_PREFIX + scope : scope)) return;
                }
                context.Result = new ObjectResult("No matching scope found: " + string.Join(", ", _scopes)) { StatusCode = 403 };
            }
        }
    }

}

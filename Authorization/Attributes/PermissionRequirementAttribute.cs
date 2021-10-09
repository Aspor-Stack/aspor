using Aspor.Authorization.Extensions;
using Aspor.Authorization.Schema;
using Aspor.Authorization.Schema.Node;
using Aspor.Authorization.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Aspor.Authorization.Attributes
{
    public class PermissionRequirementAttribute : Attribute, IAuthorizationFilter
    {

        private readonly IPermissionNode _root;

        public PermissionRequirementAttribute(string permission)
        {
            //_root = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AsporUser user = context.HttpContext.GetUser();
            PermissionSchema schema = context.HttpContext.RequestServices.GetRequiredService<PermissionSchema>();

            if (!schema.HasPermission(user, _root))
            {
                context.Result = new ObjectResult("") { StatusCode = 403 };
            }
        }
    }

}

using Aspor.Authorization.Extensions;
using Aspor.Authorization.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Aspor.Authorization.Attributes
{
    public class TypeRequirement : Attribute, IAuthorizationFilter
    {

        private readonly UserType _type;

        public TypeRequirement(UserType type)
        {
            _type = type;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            AsporUser user = context.HttpContext.GetUser();
            if (user.Type != _type)
            {
                context.Result = new ObjectResult(user.Type + " token type is not allowed to access this api method") { StatusCode = 403 };
            }
        }
    }

}

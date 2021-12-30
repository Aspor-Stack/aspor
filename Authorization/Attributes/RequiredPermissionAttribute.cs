using Aspor.Authorization.Extensions;
using Aspor.Authorization.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Authorization.Permission
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RequiredPermissionAttribute : ActionFilterAttribute
    {

        private readonly string _permission;
        private readonly string _parameter;

        public RequiredPermissionAttribute(string permission, string parameter = null)
        {
            _permission = permission;
            _parameter = parameter;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IPermissionVerifier verifier = context.HttpContext.RequestServices.GetRequiredService<IPermissionVerifier>();
            AsporUser user = context.HttpContext.GetUser();

            object parameter = null;
            if(this._parameter != null && !context.ActionArguments.TryGetValue(this._parameter, out parameter))
            {
                throw new InvalidOperationException("route parameter " + this._parameter + " not found");
            }

            bool hasPermission = await verifier.HasPermission(user, this._permission, parameter);

            if (hasPermission)
            {
                await base.OnActionExecutionAsync(context, next);
            }
            else
            {
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = "User has not enough permissions"
                };
            }
        }

    }
}

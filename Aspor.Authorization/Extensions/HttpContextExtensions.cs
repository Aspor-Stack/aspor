using Aspor.Authorization.User;
using Authorization.Permission;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Aspor.Authorization.Extensions
{

    public static class HttpContextExtensions
    {

        public static IPermissionVerifier GetPermissionVerifier(this HttpContext context)
        {
            return context.RequestServices.GetRequiredService<IPermissionVerifier>();
        }

        public static AsporUser GetUser(this HttpContext context)
        {
            AsporUser user = (AsporUser)context.Features[typeof(AsporUser)];
            if (user == null) throw new System.InvalidOperationException("Aspor user not available");
            return user;
        }

        public static async Task<bool> HasPermission(this HttpContext context, string permission, object key = null)
        {
            IPermissionVerifier verifier = GetPermissionVerifier(context);
            AsporUser user = (AsporUser)context.Features[typeof(AsporUser)];
            if (user == null) throw new System.InvalidOperationException("Aspor user not available");
            return await verifier.HasPermission(user,permission,key);
        }

        public static AsporUser GetUserOrDefault(this HttpContext context)
        {
            return (AsporUser)context.Features[typeof(AsporUser)];
        }

    }

}

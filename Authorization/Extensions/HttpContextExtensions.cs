using Aspor.Authorization.User;
using Microsoft.AspNetCore.Http;

namespace Aspor.Authorization.Extensions
{

    public static class HttpContextExtensions
    {

        public static AsporUser GetUser(this HttpContext context)
        {
            AsporUser user = (AsporUser)context.Features[typeof(AsporUser)];
            if (user == null) throw new System.InvalidOperationException("Aspor user not available");
            return user;
        }

        public static AsporUser GetUserOrDefault(this HttpContext context)
        {
            return (AsporUser)context.Features[typeof(AsporUser)];
        }

    }

}

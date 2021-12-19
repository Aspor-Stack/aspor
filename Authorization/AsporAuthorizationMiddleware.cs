using Aspor.Authorization.User;
using Aspor.Authorization.User.Factory;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Aspor.Authorization
{
    public class AsporAuthorizationMiddleware
    {

        private readonly IUserFactory _factory;
        private readonly RequestDelegate _next;

        public AsporAuthorizationMiddleware(IUserFactory factory, RequestDelegate next)
        {
            _factory = factory;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                AsporUser user = _factory.Create(context);

                if (user == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid authorization format");
                    return;
                }

                context.Features[typeof(AsporUser)] = user;
            }
            await _next(context);
        }

    }
}

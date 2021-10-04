using Aspor.Authorization.Schema;
using Aspor.Authorization.User;
using Aspor.Authorization.User.Factory;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aspor.Authorization
{
    public class AsporAuthorizationMiddleware
    {

        private readonly IUserFactory _factory;
        private readonly RequestDelegate _next;
        private readonly PermissionSchema _schema;

        public AsporAuthorizationMiddleware(IUserFactory factory, PermissionSchema schema, RequestDelegate next)
        {
            _factory = factory;
            _next = next;
            _schema = schema;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User != null)
            {
                AsporUser user = _factory.Create(context);

                if (user == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid authorization format");
                    return;
                }

                context.Features[typeof(AsporUser)] = user;

                IList<string> permissions = new List<string>();
                _schema.AppendPermissions(user, permissions);
            }
            await _next(context);
        }

    }
}

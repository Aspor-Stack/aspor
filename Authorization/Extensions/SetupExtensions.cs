using Aspor.Authorization.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aspor.Authorization.Extensions
{

    public static class SetupExtensions
    {

        public static IServiceCollection AddPermissionSchema(this IServiceCollection services, string file = "permissions.json")
        {
            return services.AddSingleton(PermissionSchemaReader.ReadFromFile(file));
        }
        public static IApplicationBuilder UseAsporAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AsporAuthorizationMiddleware>();
        }
    }

}

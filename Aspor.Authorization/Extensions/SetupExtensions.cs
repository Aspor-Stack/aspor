using Authorization.Permission;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aspor.Authorization.Extensions
{

    public static class SetupExtensions
    {

        public static IApplicationBuilder UseAsporAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AsporAuthorizationMiddleware>();
        }

        public static IServiceCollection AddPermissionVerifier(this IServiceCollection collection, IPermissionVerifier verifier)
        {
            collection.AddSingleton<IPermissionVerifier>(verifier);
            return collection;
        }

    }

}

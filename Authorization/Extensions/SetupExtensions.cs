using Authorization.Permission;
using Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.Edm;

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

        public static IServiceCollection AddPolicy(this IServiceCollection collection, QueryPolicyContext context)
        {
            collection.AddSingleton(context);
            return collection;
        }

        public static IServiceCollection AddPolicyBinder(this IServiceCollection collection, QueryPolicyContext context)
        {
            collection.Replace(new ServiceDescriptor(typeof(ISelectExpandBinder), (obj)=> new PolicySelectExpandBinder(context), ServiceLifetime.Singleton));
            return collection;
        }

        public static ODataOptions AddRouteComponents(this ODataOptions options, string routePrefix, IEdmModel model, QueryPolicyContext context)
        {
            options.AddRouteComponents(routePrefix, model, (collection) =>
            {
                collection.AddPolicyBinder(context);
            });
            return options;
        }

    }

}

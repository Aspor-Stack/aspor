using Aspor.Authorization.Policy;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aspor.EF.Extensions
{
    public static class SetupExtension
    {

        public static IServiceCollection AddDeletedEntitiesFilterSelectExpandBinder(this IServiceCollection collection)
        {
            collection.Replace(new ServiceDescriptor(typeof(ISelectExpandBinder), typeof(DeletedEntitiesFilterSelectExpandBinder), ServiceLifetime.Singleton));
            return collection;
        }

    }
}

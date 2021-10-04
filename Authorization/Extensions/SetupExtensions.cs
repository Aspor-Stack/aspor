using Aspor.Authorization.Schema;
using Microsoft.Extensions.DependencyInjection;

namespace Aspor.Authorization.Extensions
{

    public static class SetupExtensions
    {

        public static IServiceCollection AddPermissionSchema(this IServiceCollection services, string file = "permissions.json")
        {
            services.AddSingleton(PermissionSchemaReader.ReadFromFile(file));
            return services;
        }

    }

}

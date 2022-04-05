using Microsoft.OData.ModelBuilder;
using System;

namespace Aspor.Common.Extensions
{
    public static class ODataConventionModelBuilderExtensions
    {
        public static EntityTypeConfiguration<TEntity> Configure<TEntity>(this EntityTypeConfiguration<TEntity> config, Action<EntityTypeConfiguration<TEntity>> options) where TEntity : class
        {
            options.Invoke(config);
            return config;
        }

        public static ActionConfiguration Configure(this ActionConfiguration config, Action<ActionConfiguration> options)
        {
            options.Invoke(config);
            return config;
        }

        public static FunctionConfiguration Configure(this FunctionConfiguration config, Action<FunctionConfiguration> options)
        {
            options.Invoke(config);
            return config;
        }

    }
}

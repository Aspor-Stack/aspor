using Microsoft.AspNetCore.OData.Batch;
using Microsoft.Extensions.DependencyInjection;

namespace Aspor.Common.Extensions
{

    public static class SetupExtensions
    {

        public static IServiceCollection AddODataBatchHandler(this IServiceCollection services)
        {
            services.AddSingleton<ODataBatchHandler, DefaultODataBatchHandler>();
            return services;
        }

        public static IMvcBuilder AddAsporODataPageSize(this IMvcBuilder mvc, int pageSize = 1000)
        {
            mvc.AddMvcOptions(config =>
            {
                config.Filters.Add(new AsporODataDefaultPageFilter(pageSize));
            });
            return mvc;
        }

        public static IMvcBuilder AddAsporETagAutoMatch(this IMvcBuilder mvc)
        {
            mvc.AddMvcOptions(config =>
            {
                config.Filters.Add(new AsporETagAutoMatchFilter());
            });
            return mvc;
        }

        public static IMvcBuilder AddAsporReturnPreference(this IMvcBuilder mvc, string defaultReturnPreference = ReturnPreference.MINIMAL)
        {
            mvc.AddMvcOptions(config =>
            {
                config.Filters.Add(new AsporReturnPreferenceFilter(defaultReturnPreference));
            });
            return mvc;
        }

    }

}

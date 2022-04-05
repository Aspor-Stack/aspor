using Microsoft.Extensions.DependencyInjection;

namespace Aspor.Export.Extensions
{

    public static class SetupExtensions
    {

        public static IMvcBuilder AddAsporExport(this IMvcBuilder mvc, int pageSize = 10000, int maxPageSize = 0)
        {
            mvc.AddMvcOptions(config =>
            {
                int max = maxPageSize == 0 ? pageSize : maxPageSize < 0 ? int.MaxValue : maxPageSize;
                config.Filters.Add(new AsporODataExportFilter(pageSize, max));
            });
            return mvc;
        }

    }

}

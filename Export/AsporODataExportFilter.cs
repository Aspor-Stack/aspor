using Aspor.Export.Formats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.Extensions.Primitives;
using Microsoft.OData.Edm;
using System.Linq;
using System.Threading.Tasks;

namespace Aspor.Export
{

    public class AsporODataExportFilter : IAsyncActionFilter
    {

        private readonly int _pageSize;
        private readonly int _maxPageSize;

        public AsporODataExportFilter(int pageSize, int maxPageSize)
        {
            _pageSize = pageSize;
            _maxPageSize = maxPageSize;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            StringValues export = "";
            bool exportEnabled = context.HttpContext.Request.Query.TryGetValue("export", out export);

            EnableQueryAttribute queryAttribute = context.Filters.FirstOrDefault(e => e is EnableQueryAttribute) as EnableQueryAttribute;
            if (exportEnabled && queryAttribute != null)
            {
                queryAttribute.MaxTop = _maxPageSize;
                if (!context.HttpContext.Request.Query.ContainsKey("$top"))
                {
                    queryAttribute.PageSize = _pageSize;
                }
            }

            ActionExecutedContext executedContext = await next();
            if (executedContext.Canceled) return;

            if (!context.ModelState.IsValid)
            {
                executedContext.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            if (!exportEnabled) return;

            IODataFeature odataFeature = context.HttpContext.ODataFeature();
            if (odataFeature != null && odataFeature.Path != null && executedContext.Result != null)
            {
                string[] parts = export.ToString().Split("@");

                IExportFormatWriter writer = ExportFormatRegistry.GetFormatWriter(parts[0]);
                if (writer == null)
                {
                    context.ModelState.AddModelError("export", parts[0] + " is not a supported export format");
                    executedContext.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }

                IEdmModel model = context.HttpContext.Request.GetModel();
                IEdmType elementType = odataFeature.Path.GetEdmType();
                if (elementType != null) elementType = elementType.AsElementType();
                if (elementType == null || !(elementType is EdmStructuredType) || context.HttpContext.Request.IsCountRequest()) return;

                IQueryable queryable = ((ObjectResult)executedContext.Result).Value as IQueryable;
                if (queryable == null)
                {
                    context.ModelState.AddModelError("export", "Reuslt is not part of a queryable object");
                    executedContext.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }

                byte[] bytes = ExportHelper.WriteExport(writer, model, elementType as EdmStructuredType, odataFeature.SelectExpandClause, queryable);
                FileContentResult result = new FileContentResult(bytes, "application/octet-stream");
                result.FileDownloadName = parts.Length > 1 ? parts[1] : "export." + writer.GetDefaultEnding();
                executedContext.HttpContext.Response.ContentLength = bytes.Length;
                executedContext.Result = result;
            }

        }
    }
}

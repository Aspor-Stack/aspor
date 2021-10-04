using Aspor.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Results;
using System.Linq;
using System.Threading.Tasks;

namespace Aspor.Common
{

    public class AsporETagAutoMatchFilter : IAsyncActionFilter, IOrderedFilter
    {
        public int Order => 1;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext executedContext = await next();
            if (!executedContext.Canceled && context.HttpContext.Request.Method.Equals("GET") && executedContext.Result is ObjectResult result)
            {
                if (result.Value is SingleResult sResult)
                {
                    if (context.HttpContext.IFNonMatch(sResult.Queryable as IQueryable<object>))
                    {
                        executedContext.Result = new StatusCodeResult(StatusCodes.Status304NotModified);
                        return;
                    }
                }
            }
        }
    }
}

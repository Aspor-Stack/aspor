using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Aspor.Common
{

    public class AsporReturnPreferenceFilter : IAsyncActionFilter
    {

        private readonly string _defaultReturnPreference;

        public AsporReturnPreferenceFilter(string defaultReturnPreference)
        {
            _defaultReturnPreference = defaultReturnPreference;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext executedContext = await next();
            string method = context.HttpContext.Request.Method;
            //method.Equals("POST") || => Not sure if we should include post requests
            if (!executedContext.Canceled && (method.Equals("PATCH") || method.Equals("PUT") || method.Equals("DELETE")))
            {
                if (executedContext.Result is ObjectResult)
                {
                    StringValues prefer;
                    if (context.HttpContext.Request.Headers.TryGetValue("Prefer", out prefer))
                    {
                        if (prefer.ToString().Contains("return=" + ReturnPreference.MINIMAL))
                        {
                            executedContext.Result = new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                    }
                    else if (_defaultReturnPreference.Equals(ReturnPreference.MINIMAL))
                    {
                        executedContext.Result = new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                }
            }
        }

    }
}

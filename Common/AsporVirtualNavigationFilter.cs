using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace Aspor.Common
{

    public class AsporVirtualNavigationFilter : IAsyncActionFilter
    {

        private readonly string _defaultReturnPreference;

        public AsporVirtualNavigationFilter(string defaultReturnPreference)
        {
            _defaultReturnPreference = defaultReturnPreference;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
        }

    }
}

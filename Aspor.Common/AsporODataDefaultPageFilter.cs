﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;
using System.Threading.Tasks;

namespace Aspor.Common
{

    public class AsporODataDefaultPageFilter : IAsyncActionFilter, IOrderedFilter
    {
        public int Order => 2;

        private readonly int _pageSize;
        private readonly int _maxExpansionDepth;

        public AsporODataDefaultPageFilter(int pageSize, int maxExpansionDepth)
        {
            _pageSize = pageSize;
            _maxExpansionDepth = maxExpansionDepth;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (context.ModelState.IsValid)
            {
                EnableQueryAttribute queryAttribute = context.Filters.FirstOrDefault(e => e is EnableQueryAttribute) as EnableQueryAttribute;
                if (queryAttribute != null)
                {
                    if (queryAttribute.MaxExpansionDepth > _maxExpansionDepth) queryAttribute.MaxExpansionDepth = _maxExpansionDepth;
                    if (!context.HttpContext.Request.Query.ContainsKey("$top"))
                    {
                        queryAttribute.PageSize = _pageSize;
                    }
                }
            }
        }
    }
}

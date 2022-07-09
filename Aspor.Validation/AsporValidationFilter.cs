using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Extensions;
using System.Linq;
using Microsoft.OData;

namespace Aspor.Validation
{

    public class AsporValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid) {

                var error = new SerializableError();
                error["ErrorCode"] = "400";
                foreach (var pair in context.ModelState)
                {
                    var key = pair.Key;
                    var errors = pair.Value.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        var errorMessages = errors.Select(error =>
                        {
                            string message = "";
                            if (error is ModelError && (error.Exception is ODataException || error.Exception is AggregateException))
                            {
                                message = String.IsNullOrEmpty(error.ErrorMessage) ? error.Exception.Message : error.ErrorMessage;
                            }
                            return string.IsNullOrEmpty(message) ? "The input was not valid" : message;
                        }).ToArray();

                        error.Add(key, errorMessages);
                    }
                }
                context.Result = new BadRequestObjectResult(error.CreateODataError());
            }
            else
            {
                context.HttpContext.Items["modelState"] = context.ModelState;
                await next();
            }
        }
    }
}

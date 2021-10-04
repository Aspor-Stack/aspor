using Aspor.Validation;
using Aspor.Validation.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.UriParser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aspor.Streaming
{

    public class AsporODataStreamingFilter : IAsyncActionFilter
    {

        private readonly IStreamProvider _streamProvider = new DefaultStreamProvider();

        //users.{key}.create
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext executedContext = await next();
            ValidationAction? action = context.HttpContext.GetActionOrNull();
            //@Todo check if streaming enabled

            IODataFeature odataFeature = context.HttpContext.ODataFeature();
            if (action != null && odataFeature != null && odataFeature.Path != null)
            {
                ObjectResult result = executedContext.Result as ObjectResult;
                if (result != null && result.Value != null)
                {
                    StreamData data = new StreamData();
                    data.Action = (ValidationAction)action;
                    data.Instance = result.Value;
                    data.Topic = odataFeature.Path.ToString();

                    string path = context.HttpContext.Request.Path.Value;

                    string topic = "";
                    OperationSegment segment = odataFeature.Path.LastSegment as OperationSegment;
                    if (segment != null)
                    {

                    }

                    if (action == ValidationAction.UPDATE || action == ValidationAction.REPLACE)
                    {
                        data.AffectedProperties = GetChangedProperties(context);
                    }

                    _streamProvider.Publish(data);
                }
            }
        }

        public IEnumerable<string> GetChangedProperties(ActionExecutingContext context)
        {
            foreach (var obj in context.ActionArguments.Values)
            {
                if (obj is Delta) return ((Delta)obj).GetChangedPropertyNames();
            }
            return new List<string>();
        }

    }
}

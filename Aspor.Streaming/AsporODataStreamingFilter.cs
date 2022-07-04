using Aspor.Streaming.Core;
using Aspor.Streaming.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Results;
using Aspor.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.UriParser;

namespace Aspor.Streaming
{

    public class AsporODataAutoStreamingFilter : AsporODataStreamingFilter
    {
        public AsporODataAutoStreamingFilter(IStreamProvider streamProvider) : base(streamProvider, StreamMode.AUTO) {}
    }

    public class AsporODataStreamingFilter : IAsyncActionFilter
    {

        private readonly IStreamProvider _streamProvider;
        private readonly StreamMode _mode;

        public AsporODataStreamingFilter(IStreamProvider streamProvider, StreamMode mode = StreamMode.MANUALLY)
        {
            _streamProvider = streamProvider;
            _mode = mode;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext executedContext = await next();
            if (executedContext.Canceled) return;

            string method = context.HttpContext.Request.Method;
            if(method.Equals("POST") || method.Equals("PATCH") || method.Equals("PUT") || method.Equals("DELETE"))
            {
                IODataFeature odataFeature = context.HttpContext.ODataFeature();
                if (odataFeature != null && odataFeature.Path != null)
                {
                    object[] attributes = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(false);
                    if (attributes.Any(a => a.GetType() == typeof(DisableStreamAttribute))) return;

                    StreamTopicAttribute topicAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(StreamTopicAttribute)) as StreamTopicAttribute;

                    if (topicAttribute == null && _mode != StreamMode.AUTO) return;

                    string topic = topicAttribute?.Topic ?? GetPath(context.HttpContext, odataFeature);
                    object content = GetContent(executedContext.Result);
                    if(content != null && topic != null)
                    {
                        _streamProvider.PublishObject(topic, content, GetChangedProperties(context));
                    }
                }
            }
        }

        private string GetPath(HttpContext context, IODataFeature odataFeature)
        {
            string result = context.Request.Path.Value.Substring(1).Replace(odataFeature.RoutePrefix+"/","").Replace("/", ".");
            if (context.Request.Method.Equals("PATCH") || context.Request.Method.Equals("PUT")) result += ".update";
            else if (context.Request.Method.Equals("DELETE")) result += ".delete";
            else if (context.Request.Method.Equals("POST") && odataFeature.Path.LastSegment is EntitySetSegment) result += ".create";
            return result;
        }

        private object GetContent(IActionResult result)
        {
            if (result is ObjectResult) return ((ObjectResult)result).Value;
            else if (result != null && result.GetType().IsGenericType)
            {
                Type type = result.GetType().GetGenericTypeDefinition();
                if (type == typeof(CreatedODataResult<>) || type == typeof(UpdatedODataResult<>))
                {
                    return result.GetType().GetProperty("Entity").GetValue(result);
                }
            }
            return null;
        }

        private IEnumerable<string> GetChangedProperties(ActionExecutingContext context)
        {
            foreach (var obj in context.ActionArguments.Values)
            {
                if (obj is Delta) return ((Delta)obj).GetChangedPropertyNames();
            }
            return new List<string>();
        }

    }
}

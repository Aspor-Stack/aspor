
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aspor.Streaming.Attributes
{
    public class AsporStreamingAttribute : ActionFilterAttribute
    {

        private readonly bool _enabled;
        private readonly string _topic;

        public AsporStreamingAttribute(bool enabled, string topic)
        {
            _enabled = enabled;
            _topic = topic;
        }

    }
}

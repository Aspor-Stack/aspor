using System;

namespace Aspor.Streaming.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class StreamTopicAttribute : Attribute
    {

        public StreamTopicAttribute()  {}

        public StreamTopicAttribute(string topic)
        {
            Topic = topic;
        }

        public string Topic { get; }

    }
}

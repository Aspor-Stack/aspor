using System;

namespace Aspor.Streaming.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Method,AllowMultiple=true)]
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

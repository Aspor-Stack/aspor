using System;

namespace Aspor.Streaming.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromTopicAttribute : Attribute
    {
        public FromTopicAttribute(string name = null)
        {
            Name = name;
        }

        public string Name { get; }
    }
}

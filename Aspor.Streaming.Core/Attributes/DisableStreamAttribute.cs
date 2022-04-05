using System;

namespace Aspor.Streaming.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class DisableStreamAttribute : Attribute
    {

        public DisableStreamAttribute()  {}

    }
}

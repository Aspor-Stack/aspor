using Aspor.Streaming.Core.Content;
using System.Collections.Generic;

namespace Aspor.Streaming.Core
{
    public class StreamContext<T> : StreamContext where T : IStreamContent, new()
    {

        public T Content { get; set; }

    }

    public class StreamContext
    {

        public StreamData Data { get; set; }

        public IDictionary<string,string> Parameters { get; set; }

    }
}

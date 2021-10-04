using System;

namespace Aspor.Streaming
{
    public class DefaultStreamProvider : IStreamProvider
    {
        public void Publish(StreamData data)
        {
            Console.WriteLine();
        }
    }
}

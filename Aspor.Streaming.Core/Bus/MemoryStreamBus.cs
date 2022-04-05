using Aspor.Streaming.Core;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Aspor.Streaming.Core.Bus
{
    public class MemoryStreamBus : IStreamBus
    {

        private readonly ConcurrentQueue<StreamData> _queue = new ConcurrentQueue<StreamData>();
        private CancellationTokenSource _token;

        public void InitzializeHandler(IStreamReceiveHandler handler)
        {
            _token = new CancellationTokenSource();
            CancellationToken ct = _token.Token;

            var task = Task.Run(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    if(_queue.TryDequeue(out StreamData data))
                    {
                        try
                        {
                            handler.OnReceive(data);
                        }catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }, _token.Token);
        }

        public void Publish(StreamData data)
        {
            _queue.Enqueue(data);
        }

        public void Subscribe(string topic)
        {
            //Unused in memory bus
        }
    }
}

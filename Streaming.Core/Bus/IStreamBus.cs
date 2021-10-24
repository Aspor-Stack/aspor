
namespace Aspor.Streaming.Core.Bus
{
    public interface IStreamBus
    {

        public void InitzializeHandler(IStreamReceiveHandler handler);

        public void Publish(StreamData data);

        public void Subscribe(string topic);
    }
}

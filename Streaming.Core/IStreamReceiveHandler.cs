
namespace Aspor.Streaming.Core
{
    public interface IStreamReceiveHandler
    {

        public void OnReceive(StreamData data);

    }
}

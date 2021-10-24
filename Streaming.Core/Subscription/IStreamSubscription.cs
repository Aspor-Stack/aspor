
namespace Aspor.Streaming.Core.Subscription
{
    public interface IIStreamSubscription
    {

        public string[] Nodes { get; }

        public string[] NormalizedNodes { get; }

        public void Invoke(StreamData context);

    }
}

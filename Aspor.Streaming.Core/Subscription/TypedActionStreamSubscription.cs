using Aspor.Streaming;
using Aspor.Streaming.Core.Content;
using System;

namespace Aspor.Streaming.Core.Subscription
{
    public class TypedActionStreamSubscription<T> : IIStreamSubscription where T : IStreamContent, new()
    {

        private readonly Action<StreamContext<T>> _action;

        public TypedActionStreamSubscription(string[] nodes, string[] noramlizedNodes, Action<StreamContext<T>> action)
        {
            Nodes = nodes;
            NormalizedNodes = noramlizedNodes;
            _action = action;
        }

        public string[] NormalizedNodes { get; }

        public string[] Nodes { get; }

        public void Invoke(StreamData data)
        {
            StreamContext<T> context = new StreamContext<T>()
            {
                Data = data,
                Parameters = TopicMatcher.ExtractParameters(Nodes, data.Topic),
                Content = IStreamContent.CreateAndDecode<T>(data.Body)
            };
            _action.Invoke(context);
        }
    }
}

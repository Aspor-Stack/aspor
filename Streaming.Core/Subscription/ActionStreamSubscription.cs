using System;

namespace Aspor.Streaming.Core.Subscription
{
    public class ActionStreamSubscription : IIStreamSubscription
    {

        private readonly Action<StreamContext> _action;

        public ActionStreamSubscription(string[] nodes, string[] noramlizedNodes, Action<StreamContext> action)
        {
            Nodes = nodes;
            NormalizedNodes = noramlizedNodes;
            _action = action;
        }

        public string[] NormalizedNodes { get; }

        public string[] Nodes { get; }

        public void Invoke(StreamData data)
        {
            StreamContext context = new StreamContext()
            {
                Data = data,
                Parameters = TopicMatcher.ExtractParameters(Nodes, data.Topic)
            };
            _action.Invoke(context);
        }

        public void Remove()
        {
            throw new System.NotImplementedException();
        }
    }
}

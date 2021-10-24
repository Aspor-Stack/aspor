using Aspor.Streaming.Core.Attributes;
using Newtonsoft.Json.Linq;
using Aspor.Streaming.Core;
using Aspor.Streaming.Core.Bus;
using Aspor.Streaming.Core.Content;
using Aspor.Streaming.Core.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aspor.Streaming.Core
{
    public class DefaultStreamProvider : IStreamProvider, IStreamReceiveHandler
    {

        private IStreamBus _bus;
        private readonly ICollection<IIStreamSubscription> _subscriptions;

        public DefaultStreamProvider()
        {
            _subscriptions = new List<IIStreamSubscription>();
        }

        public DefaultStreamProvider(IStreamBus bus)
        {
            _subscriptions = new List<IIStreamSubscription>();
            _bus = bus;
            _bus.InitzializeHandler(this);
        }

        public void UseBus(IStreamBus bus)
        {
            _bus = bus;
            _bus.InitzializeHandler(this);
        }

        public void Publish(string topic, string content)
        {
            Publish(topic, new StringStreamContent(content));
        }

        public void PublishObject<E>(string topic, E content) where E : class
        {
            Publish(topic, new ObjectStreamContent<E>(content));
        }

        public void PublishObject<E>(string topic, E content, IEnumerable<string> affectedProperties) where E : class
        {
            Publish(topic, new EntityStreamContent<E>(content, affectedProperties));
        }

        public void Publish(string topic, JToken content)
        {
            Publish(topic, new JsonStreamContent(content));
        }

        public void Publish(string topic, byte[] content)
        {
            Publish(topic, new ByteStreamContent(content));
        }

        public void Publish(string topic, IStreamContent content)
        {
            if (_bus == null) throw new InvalidOperationException("No bus available");
            StreamData data = new StreamData()
            {
                Topic = topic,
                Body = content.Encode(),
            };
            _bus.Publish(data);
        }

        public void Consume(string topic, Action<StreamContext> consumer)
        {
            if (_bus == null) throw new InvalidOperationException("No bus available");
            string[] nodes = TopicMatcher.Compute(topic);
            string[] normalizedNodes = TopicMatcher.Noramlize(nodes);
            _subscriptions.Add(new ActionStreamSubscription(nodes, normalizedNodes, consumer));
            _bus.Subscribe(string.Join(TopicMatcher.NODE_DELIMITER, normalizedNodes));
        }

        public void Consume<T>(string topic, Action<StreamContext<T>> consumer) where T : IStreamContent, new()
        {
            if (_bus == null) throw new InvalidOperationException("No bus available");
            string[] nodes = TopicMatcher.Compute(topic);
            string[] normalizedNodes = TopicMatcher.Noramlize(nodes);
            _subscriptions.Add(new TypedActionStreamSubscription<T>(nodes, normalizedNodes, consumer));
            _bus.Subscribe(string.Join(TopicMatcher.NODE_DELIMITER, normalizedNodes));
        }

        public void OnReceive(StreamData data)
        {
            foreach (IIStreamSubscription subscription in _subscriptions)
            {
                if (TopicMatcher.Matches(subscription.NormalizedNodes, data.Topic))
                {
                    subscription.Invoke(data);
                }
            }
        }

        public void Consume<T>() where T : class, new()
        {
            Consume(new T());
        }

        public void Consume(Type type)
        {
            Consume(Activator.CreateInstance(type));
        }

        private void Consume(object listener)
        {
            if (_bus == null) throw new InvalidOperationException("No bus available");
            IEnumerable<MethodInfo> methods = listener.GetType().GetMethods().Where(m => !m.IsStatic
                && m.IsPublic
                && m.GetCustomAttributes(false).Any(c => c.GetType() == typeof(StreamTopicAttribute)));

            foreach (MethodInfo method in methods)
            {
                StreamTopicAttribute topic = method.GetCustomAttribute<StreamTopicAttribute>();
                string[] nodes = TopicMatcher.Compute(topic.Topic);
                string[] normalizedNodes = TopicMatcher.Noramlize(nodes);
                _subscriptions.Add(new MethodStreamSubscription(nodes, normalizedNodes, method, listener));
                _bus.Subscribe(string.Join(TopicMatcher.NODE_DELIMITER, normalizedNodes));
            }
        }

        public void AddConsumers()
        {
            if (_bus == null) throw new InvalidOperationException("No bus available");
            var controllerType = typeof(StreamConsumer);
            var controllers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => controllerType.IsAssignableFrom(t));
            foreach (Type controller in controllers) Consume(controllers);
        }
    }
}

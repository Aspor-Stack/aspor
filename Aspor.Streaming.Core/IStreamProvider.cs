using Newtonsoft.Json.Linq;
using Aspor.Streaming.Core;
using Aspor.Streaming.Core.Bus;
using Aspor.Streaming.Core.Content;
using System;
using System.Collections.Generic;

namespace Aspor.Streaming.Core
{
    public interface IStreamProvider
    {

        public void UseBus(IStreamBus bus);

        public void Publish(string topic, string content);

        public void Publish(string topic, JToken json);

        public void Publish(string topic, byte[] content);

        public void Publish(string topic, IStreamContent content);

        public void PublishObject<E>(string topic, E obj) where E : class;

        public void PublishObject<E>(string topic, E obj, IEnumerable<string> affectedProperties) where E : class;

        public void Consume(string topic, Action<StreamContext> consumer);

        public void Consume<T>(string topic, Action<StreamContext<T>> consumer) where T : IStreamContent, new();

        public void Consume<T>() where T : class, new();

        public void Consume(Type type);

        public void Consume(object listener);

        public void AddConsumers();

    }
}

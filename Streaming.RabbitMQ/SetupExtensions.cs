using Aspor.Streaming.Core;
using RabbitMQ.Client;
using System;

namespace Aspor.Streaming.RabbitMQ
{

    public static class SetupExtensions
    {

        public static IStreamProvider UseRabbitMQ(this IStreamProvider provider, string exchange, ConnectionFactory factory)
        {
            provider.UseBus(new RabbitMQStreamBus(exchange, null, factory));
            return provider;
        }

        public static IStreamProvider UseRabbitMQ(this IStreamProvider provider, string exchange, Action<ConnectionFactory> action)
        {
            ConnectionFactory factory = new ConnectionFactory();
            action.Invoke(factory);
            return UseRabbitMQ(provider, exchange, null, factory);
        }

        public static IStreamProvider UseRabbitMQ(this IStreamProvider provider, string exchange, string queue, ConnectionFactory factory)
        {
            provider.UseBus(new RabbitMQStreamBus(exchange,queue,factory));
            return provider;
        }

        public static IStreamProvider UseRabbitMQ(this IStreamProvider provider, string exchange, string queue, Action<ConnectionFactory> action)
        {
            ConnectionFactory factory = new ConnectionFactory();
            action.Invoke(factory);
            return UseRabbitMQ(provider,exchange,queue,factory);
        }
    }

}

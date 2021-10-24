using Aspor.Streaming.Core;
using Aspor.Streaming.Core.Bus;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Aspor.Streaming.RabbitMQ
{
    public class RabbitMQStreamBus : IStreamBus
    {

        private readonly string _exchange;
        private readonly string _queue;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private IStreamReceiveHandler _handler;
        private string _activeQueue;

        public RabbitMQStreamBus(string exchange, string queue, ConnectionFactory factory)
        {
            _exchange = exchange;
            _queue = queue;
            _factory = factory;
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(exchange, "topic");
        }

        public void InitzializeHandler(IStreamReceiveHandler handler)
        {
            _handler = handler;
        }

        public void Publish(StreamData context)
        {
            _model.BasicPublish(exchange: _exchange,
                                      routingKey: context.Topic,
                                      basicProperties: null,
                                      body: context.Body);
        }

        public void Subscribe(string topic)
        {
            if(_activeQueue == null)
            {
                if (_queue == null) _activeQueue = _model.QueueDeclare().QueueName;
                else _activeQueue = _model.QueueDeclare(_queue,false,false,false).QueueName;
                var consumer = new EventingBasicConsumer(_model);
                consumer.Received += (model, args) => _handler.OnReceive(new StreamData()
                {
                    Topic = args.RoutingKey,
                    Body = args.Body.ToArray()
                });
                _model.BasicConsume(queue: _activeQueue, autoAck: true, consumer: consumer);
            }
            _model.QueueBind(_activeQueue, _exchange, topic);
        }
    }
}

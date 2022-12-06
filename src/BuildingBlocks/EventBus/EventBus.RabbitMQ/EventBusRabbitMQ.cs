using System;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        RabbitMQPersistentConnection rabbitMQPersistentConnection;
        IConnectionFactory _connectionFactory;
        private readonly IModel _consumerChannel;

        public EventBusRabbitMQ(EventBusConfig config, IServiceProvider serviceProvider) : base(config, serviceProvider)
        {
            if (config.Connection != null)
            {
                if (EventBusConfig.Connection is ConnectionFactory)
                    _connectionFactory = EventBusConfig.Connection as ConnectionFactory;
                else
                {
                    var connJson = JsonConvert.SerializeObject(EventBusConfig.Connection, new JsonSerializerSettings()
                    {
                        // Self referencing loop detected for property 
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    _connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
                }
            }
            else
                _connectionFactory = new ConnectionFactory();

            rabbitMQPersistentConnection =
                new RabbitMQPersistentConnection(_connectionFactory, config.ConnectionRetryCount);
            _consumerChannel = CreateConsumerChannel();

            SubsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object? sender, string eventName)
        {
            eventName = ProcessEventName(eventName);
            if (!rabbitMQPersistentConnection.IsConnected)
                rabbitMQPersistentConnection.TryConnect();

            _consumerChannel.QueueUnbind(queue: eventName, exchange: EventBusConfig.DefaultTopicName,
                routingKey: eventName);

            if (SubsManager.IsEmpty)
                _consumerChannel.Close();
        }

        public override void Publish(IntegrationEvent @event)
        {
            if (!rabbitMQPersistentConnection.IsConnected)
                rabbitMQPersistentConnection.TryConnect();

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(EventBusConfig.ConnectionRetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        //Logging
                    });

            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);

            _consumerChannel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName,
                type: "direct"); //Ensure exchange exists while publishing

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(() =>
            {
                var properties = _consumerChannel.CreateBasicProperties();

                properties.DeliveryMode = 2; //persistent

                // _consumerChannel.QueueDeclare(queue: GetSubName(eventName), durable: true, exclusive: false,
                //     autoDelete: false, arguments: null); //Ensure queue exists while publishing

                //_consumerChannel.QueueBind(queue: GetSubName(eventName), exchange: EventBusConfig.DefaultTopicName,
                //routingKey: eventName);

                _consumerChannel.BasicPublish(exchange: EventBusConfig.DefaultTopicName, routingKey: eventName,
                    mandatory: true, basicProperties: properties, body: body);
            });
        }

        public override void Subscribe<T, THandler>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            if (!SubsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!rabbitMQPersistentConnection.IsConnected)
                    rabbitMQPersistentConnection.TryConnect();

                _consumerChannel.QueueDeclare(queue: GetSubName(eventName), durable: true, exclusive: false,
                    autoDelete: false, arguments: null); //Ensure queue exists while consuming

                _consumerChannel.QueueBind(queue: GetSubName(eventName), exchange: EventBusConfig.DefaultTopicName,
                    routingKey: eventName);
            }

            SubsManager.AddSubscription<T, THandler>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, THandler>()
        {
            SubsManager.RemoveSubscription<T, THandler>();
        }

        private void StartBasicConsume(string eventName)
        {
            if (_consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(queue: GetSubName(eventName), autoAck: false, consumer: consumer);
            }
        }

        private async void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                //logging
            }

            _consumerChannel.BasicAck(e.DeliveryTag, multiple: false);
        }

        private IModel CreateConsumerChannel()
        {
            if (!rabbitMQPersistentConnection.IsConnected)
                rabbitMQPersistentConnection.TryConnect();

            var channel = rabbitMQPersistentConnection.CreateModel();
            channel.ExchangeDeclare(EventBusConfig.DefaultTopicName, "direct");
            return channel;
        }
    }
}
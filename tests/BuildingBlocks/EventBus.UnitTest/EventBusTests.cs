namespace EventBus.UnitTest;

[TestClass]
public class EventBusTests
{
    private readonly ServiceCollection _services;

    public EventBusTests()
    {
        _services = new ServiceCollection();
        _services.AddLogging(configure => configure.AddConsole());
    }

    [TestMethod]
    public void subscribe_event_on_rabbitmq_test()
    {
        _services.AddSingleton<IEventBus>(sp => EventBusFactory.Create(CreateEventBusConfigRabbitMq(), sp));

        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
    }

    [TestMethod]
    public void send_message_to_rabbitmq_test()
    {
        _services.AddSingleton<IEventBus>(sp => EventBusFactory.Create(CreateEventBusConfigRabbitMq(), sp));
        var sp = _services.BuildServiceProvider();
        var eventBus = sp.GetRequiredService<IEventBus>();

        eventBus.Publish(new OrderCreatedIntegrationEvent(23));
    }

   

    private EventBusConfig CreateEventBusConfigRabbitMq()
    {
        return new EventBusConfig()
        {
            ConnectionRetryCount = 5,
            DefaultTopicName = "MicroServiceProjectTopicName",
            SubscriberClientAppName = "EventBus.UnitTest",
            EventBusType = EventBusType.RabbitMQ,
            //   Connection = new ConnectionFactory(),
            // {
            //     HostName ="localhost",
            //     Port = 15672,
            //     UserName = "guest",
            //     Password = "guest"
            // },
            EventNameSuffix = "IntegrationEvent"
        };
    }
}
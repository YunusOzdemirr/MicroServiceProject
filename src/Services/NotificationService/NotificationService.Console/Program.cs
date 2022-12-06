// See https://aka.ms/new-console-template for more information

using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Console.IntegrationEvents.EventHandlers;
using NotificationService.Console.IntegrationEvents.Events;
using RabbitMQ.Client;

ServiceCollection services = new ServiceCollection();

ConfigureServices(services);

var sp = services.BuildServiceProvider();
IEventBus eventBus=sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

Console.WriteLine("Application is Running...");
Console.ReadLine();

void ConfigureServices(ServiceCollection services)
{

    services.AddLogging(configure =>
    {
        configure.Services.AddLogging();
    });

    services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();

    services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();


    services.AddSingleton<IEventBus>(sp =>
    {
        EventBusConfig config = new()
        {
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "NotificationService",
            Connection = new ConnectionFactory(),
            EventBusType = EventBusType.RabbitMQ
        };
        return EventBusFactory.Create(config, sp);
    });

}
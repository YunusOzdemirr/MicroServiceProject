using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.API.IntegrationEvents.EventHandlers;
using PaymentService.API.IntegrationEvents.Events;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(configure => configure.AddConsole());

builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
        Connection=new ConnectionFactory(),
        EventBusType=EventBusType.RabbitMQ
    };
    return EventBusFactory.Create(config, sp);
});

var app = builder.Build();


IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



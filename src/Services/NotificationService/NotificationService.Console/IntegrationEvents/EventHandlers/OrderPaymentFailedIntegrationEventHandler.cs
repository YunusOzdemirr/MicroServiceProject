using System;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Console.IntegrationEvents.Events;

namespace NotificationService.Console.IntegrationEvents.EventHandlers
{
	public class OrderPaymentFailedIntegrationEventHandler:IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
	{
		private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> _logger;

        public OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {
            //Send Fail Notification (Sms, Email, Push)
            _logger.LogInformation($"Order Payment failed with OrderId: {@event.OrderId}, Message: {@event.Message}");
            System.Console.WriteLine(@event.Id);
            return Task.CompletedTask;
        }
    }
}


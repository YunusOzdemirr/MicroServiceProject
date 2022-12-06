using System;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Console.IntegrationEvents.Events;

namespace NotificationService.Console.IntegrationEvents.EventHandlers
{
	public class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
	{
		private readonly ILogger<OrderPaymentSuccessIntegrationEventHandler> _logger;

        public OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            //Send Fail Notification (Sms, Email, Push)
            _logger.LogInformation($"Order Payment success with OrderId: {@event.OrderId}, Message: {@event.Message}");

            return Task.CompletedTask;
        }
    }
}


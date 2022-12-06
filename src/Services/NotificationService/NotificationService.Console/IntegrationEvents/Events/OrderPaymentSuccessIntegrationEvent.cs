using System;
using EventBus.Base.Events;

namespace NotificationService.Console.IntegrationEvents.Events
{
	public class OrderPaymentSuccessIntegrationEvent:IntegrationEvent
	{
		public int OrderId { get; set; }
		public string Message { get; set; }
		public OrderPaymentSuccessIntegrationEvent(int orderId,string message)
		{
			OrderId = orderId;
			Message = message;
		}
	}
}


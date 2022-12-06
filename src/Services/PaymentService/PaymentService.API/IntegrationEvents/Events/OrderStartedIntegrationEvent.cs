using System;
using EventBus.Base.Events;

namespace PaymentService.API.IntegrationEvents.Events
{
	public class OrderStartedIntegrationEvent:IntegrationEvent
	{
		public int OrderId { get; set; }
		public OrderStartedIntegrationEvent()
		{
		}
		public OrderStartedIntegrationEvent(int orderId)
		{
			OrderId = orderId;
		}
	}
}


﻿using System;
using EventBus.Base.Events;

namespace PaymentService.API.IntegrationEvents.Events
{
	public class OrderPaymentFailedIntegrationEvent:IntegrationEvent
	{
		public int OrderId { get; set; }
		public string Message { get; set; }
		public OrderPaymentFailedIntegrationEvent(int orderId, string message)
		{
			OrderId = orderId;
			Message = message;
		}
	}
}


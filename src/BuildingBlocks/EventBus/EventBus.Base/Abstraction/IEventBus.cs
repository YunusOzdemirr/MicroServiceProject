using System;
using EventBus.Base.Events;

namespace EventBus.Base.Abstraction
{
    public interface IEventBus:IDisposable
    {
        void Publish(IntegrationEvent @event);
        void Subscribe<T, THandler>() where T : IntegrationEvent where THandler : IIntegrationEventHandler<T>;
        void UnSubscribe<T, THandler>() where T : IntegrationEvent where THandler : IIntegrationEventHandler<T>;
    }
}


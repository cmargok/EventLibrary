using EventBusLibrary.Core.Events;
namespace EventBusLibrary.Core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);

    }

    public interface IEventHandler { }
}

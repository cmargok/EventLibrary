using EventBusCleanLibrary.Core.Events;
namespace EventBusCleanLibrary.Core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);

    }

    public interface IEventHandler { }
}

using EventBusLibrary.Core.Commands;
using EventBusLibrary.Core.Events;
namespace EventBusLibrary.Core.Bus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, EHT>() where T : Event where EHT : IEventHandler<T>;
    }




}

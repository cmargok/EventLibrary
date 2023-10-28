using EventBusCleanLibrary.Core.Events;
using System;

namespace EventBusCleanLibrary
{
   
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : Event;

      //  void Subscribe<T, EHT>() where T : Event where EHT : IEventHandler<T>;
    }
}

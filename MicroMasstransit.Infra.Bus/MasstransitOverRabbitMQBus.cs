using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using MassTransit;
using Event = MicroRabbit.Domain.Core.Events.Event;

namespace MicroMasstransit.Infra.Bus
{
    public sealed class MasstransitOverRabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handler;
        private readonly List<Type> _eventTypes;
        private readonly IBus _bus;

        public MasstransitOverRabbitMQBus(IMediator mediator, IBus bus)
        {
            _mediator = mediator;
            _handler = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _bus = bus;
        }
        public async Task Publish<T>(T @event) where T : Event
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri(""));
            await endpoint.Send(@event);

        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, EHT>()
            where T : Event
            where EHT : IEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}

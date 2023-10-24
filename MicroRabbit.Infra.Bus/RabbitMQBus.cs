
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json.Serialization;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handler;
        private readonly List<Type> _eventTypes;
        private readonly RabbitMQSettings _settings;
        public RabbitMQBus(IMediator mediator, IOptions<RabbitMQSettings> settings)
        {
            _mediator = mediator;
            _handler = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _settings = settings.Value;
        }


        public void Publish<T>(T @event) where T : Event
        {
            //definimos los parametros para la conexion
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                Port = 5672,
                UserName = _settings.UserName,//"cmargok",
                Password = _settings.Password,//"Pollito@1"
            };

       
            //creamos conexion
            using var connection = factory.CreateConnection();

            //creamos channel
            using var channel = connection.CreateModel();
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName,false,false,false,null);

                var message = JsonConvert.SerializeObject(@event);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", eventName, null, body);             

            }
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, EHT>() where T : Event where EHT : IEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}

using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly RabbitMQSettings _settings;
        public RabbitMQBus(IMediator mediator, IOptions<RabbitMQSettings> settings)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
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

        public Task SendCommand<T>(T command) where T : Command =>  _mediator.Send(command);


        public void Subscribe<T, EHT>() where T : Event where EHT : IEventHandler<T>
        {
            var eventType = typeof(T);
            var eventName = eventType.Name;

            var handlerType = typeof(EHT);

            if (!_eventTypes.Contains(eventType))
            {
                _eventTypes.Add(eventType);
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException($"el handler exception {handlerType.Name} ya fue registrado anteriormente por {eventName}", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                Port = 5672,
                UserName = _settings.UserName,//"cmargok",
                Password = _settings.Password,//"Pollito@1"
                DispatchConsumersAsync = true,
            };

             var connection = factory.CreateConnection();

            //creamos channel
             var channel = connection.CreateModel();
           


            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);


            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);



        }

        //delegado en evento
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;

            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName,message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        //consume los mensajes de la queue
        private async Task ProcessEvent(string eventName, string message)
        {

            if (_handlers.ContainsKey(eventName))
            {
                var subscriptions = _handlers[eventName];

                foreach (var subscription in subscriptions) 
                {
                    var handler = Activator.CreateInstance(subscription);

                    if (handler == null) continue;

                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);

                    var @event = JsonConvert.DeserializeObject(message, eventType!);

                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType!);

                    await (Task)concreteType.GetMethod("Handle").Invoke(handler,new object[] { @event });


                }
            }
        }
    }
}

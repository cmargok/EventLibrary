using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Consumer
{
    internal class Receiver
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.0.13",
                Port = 5672,
                UserName = "cmargok",
                Password = "Pollito@1"
            };
            using var connection = factory.CreateConnection();

            //creamos channel
            using var channel = connection.CreateModel();
            {
                //creamos la cola
                channel.QueueDeclare("KevQueue", false, false, false, null);
                
                //creamos el evento para atrapar lo que hay en el canal
                var consumer = new EventingBasicConsumer(channel);

                //recibimos el mensaje por medio de un delegado
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.Span;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"mensaje enviado {message}");
                };

                //mensaje leido mensaje quitado
                channel.BasicConsume("KevQueue", true, consumer);

                Console.WriteLine("Presiona enter para salir");
                Console.ReadLine();

            }
        }
    }
}

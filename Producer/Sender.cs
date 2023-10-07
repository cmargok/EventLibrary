using RabbitMQ.Client;
using System.Text;

namespace Producer
{
    internal class Sender
    {
        static void Main(string[] args)
        {
            //definimos los parametros para la conexion
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.0.13",
                Port = 5672,
                UserName = "cmargok",
                Password = "Pollito@1"
            }; 
            
            Console.WriteLine("Presiona enter para enviar mensaje");

            Console.ReadLine();

            //creamos conexion
            using var connection = factory.CreateConnection();

            //creamos channel
            using var channel = connection.CreateModel();
            {
                //creamos la cola
                channel.QueueDeclare("KevQueue", false, false, false, null);

                //objeto a enviar
                var msg = "bienvenido al curso de rabbit";

                //codificamos a bytes el objeto
                var body = Encoding.UTF8.GetBytes(msg);

                //enviamos el mensaje a la cola, en este caso el primer parametro es el exchange q va vacio
                //para que rabbitmq lo genere, el nombre del routing key, q como es direct exchange usamos
                // el mismo nombre de la cola, null para las propiedades y el objeto en bytes
                channel.BasicPublish("", "KevQueue", null, body);


                Console.WriteLine($"mensaje enviado {msg}");
            }

            Console.WriteLine("Presiona enter para salir");
            Console.ReadLine();
        }
    }
}

using EventBusLibrary.Bus;
using EventBusLibrary.Core.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusLibrary.IoC
{
    public class DependecyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {

            services.AddTransient<IEventBus, RabbitMQBus>();

            services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQSettings"));
        }
    }
}

using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroRabbit.Infra.IoC
{
    public class DependecyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration config)
        {

            services.AddTransient<IEventBus, RabbitMQBus>();

            services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQSettings"));

            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}

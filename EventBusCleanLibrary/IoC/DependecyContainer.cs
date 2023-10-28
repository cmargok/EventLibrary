using EventBusCleanLibrary.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EventBusCleanLibrary.IoC
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration config)
        {

          //  services.AddTransient<IEventBus, RabbitMQBus>();

            services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQSettings"));

            services.AddMediatR(config =>
           config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddMediatR(config =>
               config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}

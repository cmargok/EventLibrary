using EventBusCleanLibrary.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventBusCleanLibrary.IoC
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration config)
        {

            services.AddSingleton<IEventBus, RabbitMQBus>(opt =>
            {
                var serviceScopeFactory = opt.GetRequiredService<IServiceScopeFactory>();

                var options = opt.GetService<IOptions<RabbitMQSettings>>();

                if (serviceScopeFactory is null) throw new ArgumentNullException(nameof(serviceScopeFactory));

                return new RabbitMQBus(options!, serviceScopeFactory);
            });
            var h = config.GetSection("RabbitMQSettings");
            services.Configure<RabbitMQSettings>(h);          

            return services;
        }
    }
}

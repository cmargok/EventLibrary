using EventBusLibrary.Bus;
using EventBusLibrary.Core.Bus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace EventBusLibrary.IoC
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration config)
        {

            services.AddSingleton<IEventBus, RabbitMQBus>(opt =>
            {
                var mediator = opt.GetService<IMediator>();

                var serviceScopeFactory = opt.GetRequiredService<IServiceScopeFactory>();

                var options = opt.GetService<IOptions<RabbitMQSettings>>();

                if (mediator is null) throw new ArgumentNullException(nameof(mediator));

                if (serviceScopeFactory is null) throw new ArgumentNullException(nameof(serviceScopeFactory));

                return new RabbitMQBus(mediator!, options!, serviceScopeFactory);
            });


            services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQSettings"));

            services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));



            //services.AddMediatR(config =>
            //   config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    } 
}

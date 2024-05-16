using DemoLibrary.Infraestructure.Messaging.Async;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DemoLibrary.CrossCutting
{
    public static class AsyncQueueInjection
    {
        public static IServiceCollection AddRabbitMqMessagePublisher(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMqMessagePublisher>(sp =>
            {
                // gets connection instantiated in rabbitServiceCollectionExtensions
                var rabbitConnection = sp.GetRequiredService<IConnection>();
                Console.WriteLine($"-->{rabbitConnection}");

                // adds publisher to the service collection // dependency injection
                return new RabbitMqMessagePublisher(rabbitConnection, "jamhub");
            });
            return services;
        }
    }
}
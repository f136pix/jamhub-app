using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Infraestructure.Messaging._Mail;
using DemoLibrary.Infraestructure.Messaging.Async;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DemoLibrary.CrossCutting
{
    public static class AsyncQueueInjection
    {
        public static IServiceCollection InitializeRabbitMQQueues(this IServiceCollection services, List<string> queues)
        {
            var rabbitConnection = services.BuildServiceProvider().GetService<IConnection>();
            foreach (var queue in queues)
            {
                using (var channel = rabbitConnection.CreateModel())
                {
                    // sends 1 message of any size at time // only sends next when prev is processed
                    channel.BasicQos(
                        prefetchSize: 0,
                        prefetchCount: 1,
                        global: false
                    );

                    // queue parameters
                    var args = new Dictionary<string, object>
                    {
                        { "x-max-priority", 10 }
                    };

                    // create queue if doesnt exists
                    channel.QueueDeclare(
                        queue: queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: args
                    );
                }
            }

            return services;
        }

        public static IServiceCollection AddRabbitMqMessagePublisher(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMqMessagePublisher>(sp =>
            {
                // gets connection instantiated in rabbitServiceCollectionExtensions
                var rabbitConnection = sp.GetRequiredService<IConnection>();
                Console.WriteLine($"-->Publisher connection : {rabbitConnection}");

                var rabbitMqSettings = sp.GetRequiredService<IOptions<RabbitMQSettings>>();

                // adds publisher to the service collection // dependency injection
                return new RabbitMqMessagePublisher(rabbitConnection, "dotnet.rails", rabbitMqSettings);
            });
            return services;
        }

        public static IServiceCollection AddRabbitMqMessageConsumer(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMqMessageConsumer>(sp =>
            {
                // gets connection instantiated in rabbitServiceCollectionExtensions
                var rabbitConnection = sp.GetRequiredService<IConnection>();
                Console.WriteLine($"-->Publisher connection : {rabbitConnection}");

                var asyncProcessorService = sp.GetRequiredService<IAsyncProcessorService>();

                // adds consumer to the service collection // dependency injection
                return new RabbitMqMessageConsumer(rabbitConnection, "rails.dotnet", asyncProcessorService);
            });
            return services;
        }

        public static IApplicationBuilder RunRabbitMqMessageConsumer(this IApplicationBuilder app)
        {
            // orquestrates the consumer so it runs with the application
            var consumer = app.ApplicationServices.GetRequiredService<RabbitMqMessageConsumer>();
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lifetime?.ApplicationStarted.Register(() => consumer.InitializeConsumer());

            lifetime?.ApplicationStopping.Register(() => consumer.Dispose());
            return app;
        }
    }
}
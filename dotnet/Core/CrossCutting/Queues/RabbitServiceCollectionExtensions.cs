using DemoLibrary.CrossCutting.Queues.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DemoLibrary.CrossCutting.Queues
{
    // provides helper method for adding a rabbitMq connection
    public static class RabbitServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitConnection(
            this IServiceCollection services,
            string host,
            string user,
            string password,
            string port,
            string virtualHost
        )
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddOptions();

            const int DEFAULT_SECOND = 3600;
            const int DEFAULT_TIMEOUT = DEFAULT_SECOND * 1000;

            var factory = new ConnectionFactory
            {
                HostName = host,
                UserName = user,
                Password = password,
                VirtualHost = virtualHost,
                Port = Convert.ToInt32(port),
                AutomaticRecoveryEnabled = true,
                RequestedConnectionTimeout = new TimeSpan(DEFAULT_TIMEOUT),
                RequestedHeartbeat = new TimeSpan(DEFAULT_SECOND),
                SocketWriteTimeout = new TimeSpan(DEFAULT_TIMEOUT),
                SocketReadTimeout = new TimeSpan(DEFAULT_TIMEOUT)
            };

            services.AddSingleton<ConnectionFactory>(_ => factory);
            services.AddSingleton<IConnection>(_ => factory.CreateConnection());

            return services;
        }

        public static IServiceCollection AddRabbitConnection(this IServiceCollection services, IBusConfigData config)
        {
            var rabbit = new
            {
                Host = config.Host,
                User = config.User,
                Password = config.Password,
                Port = config.Port,
                VirtualHost = config.VirtualHost
            };

            return AddRabbitConnection(
                services,
                rabbit.Host,
                rabbit.User,
                rabbit.Password,
                rabbit.Port,
                rabbit.VirtualHost
            );
        }
    }
}
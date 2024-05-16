using System.Text;
using DemoLibrary.CrossCutting.Logger;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace DemoLibrary.Infraestructure.Messaging.Async;

using DemoLibrary.Infraestructure.Messaging;

public enum RoutingKeys
{
    CreateUser,
    UpdateUser,
    DeleteUser,
}

public class RabbitMqMessagePublisher : RabbitMQBase
{
    private LoggerBase _logger;
    private readonly string _exchange;

    public RabbitMqMessagePublisher(
        IConnection rabbitConnection,
        string queue,
        string exchange
    ) :
        base(rabbitConnection, queue)
    {
        _exchange = exchange;

        _logger = new LoggerBase(logDirectory: "AMPQ");
        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        _logger.WriteLog($"Initializing RabbitMQ, queue: {_queue}");
        using (var channel = _rabbitConnection.CreateModel())
        {
            // sends 1 message of any size at time // only sends next when prev is processed
            channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false
            );

            // create exchange if not exists
            channel.ExchangeDeclare(_exchange, "direct", true);

            // queue parameters
            var args = new Dictionary<string, object>
            {
                { "x-max-priority", 10 }
            };

            // create queue if doesnt exists
            channel.QueueDeclare(
                queue: _queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args
            );
        }
    }

    public override async Task PublishAsync<T>(T message, RoutingKeys routingKey)
    {
        try
        {
            using (var channel = _rabbitConnection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                var jsonMessage = JsonConvert.SerializeObject(
                    message,
                    Formatting.None
                );

                var body = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(
                    exchange: _exchange,
                    routingKey: routingKey.ToString(),
                    basicProperties: props,
                    body: body
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("--> Exception on RabbitMQ");
            throw;
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
    }
}
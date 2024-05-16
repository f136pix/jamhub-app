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
    private RabbitMqLogger _logger;

    // ctor
    public RabbitMqMessagePublisher(
        IConnection rabbitConnection,
        string queue
    ) :
        base(rabbitConnection, queue)
    {
        Console.WriteLine($"--> RabbitMqMessagePublisher queue: {_queue}");

        _logger = new RabbitMqLogger(logDirectory: "AMPQ");
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

    public override async Task PublishAsync<T>(T message, string exchange, RoutingKeys routingKey)
    {
        try
        {
            using (var channel = _rabbitConnection.CreateModel())
            {
                // create the exchange if doesnt exsits
                channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);

                // enable confirm mode
                channel.ConfirmSelect();

                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                // to json string
                var jsonMessage = JsonConvert.SerializeObject(
                    message,
                    Formatting.None
                );

                Console.WriteLine(jsonMessage);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                Console.WriteLine(body);

                channel.BasicPublish(
                    exchange: exchange,
                    routingKey: routingKey.ToString(),
                    basicProperties: props,
                    body: body
                );

                if (!channel.WaitForConfirms())
                {
                    throw new Exception("Message not received in the Broker");
                }

                _logger.LogRabbitMqMessage(_queue, exchange, routingKey.ToString(), jsonMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("--> Exception on RabbitMQ");
            _logger.LogRabbitMqError(_queue, ex.Message);
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
    }
}
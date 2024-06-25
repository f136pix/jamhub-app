using System.Text;
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Infraestructure.Messaging._Mail;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace DemoLibrary.Infraestructure.Messaging.Async;

public class RabbitMqMessagePublisher : RabbitMQBase
{
    private RabbitMqLogger _logger;

    // ctor
    public RabbitMqMessagePublisher(
        IConnection rabbitConnection,
        string queue,
        IOptions<RabbitMQSettings> rabbitMqSettings,
        ILoggerBaseFactory loggerFactory
    ) :
        base(rabbitConnection, queue, rabbitMqSettings)
    {
        Console.WriteLine($"--> RabbitMqMessagePublisher queue: {_queue}");

        _logger = loggerFactory.CreateRabbitMqLogger("AMPQ-Publisher");
        InitializeRabbitMq();
    }

    private async Task<bool> ExchangeExists(string exchangeName)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{_rabbitMqSettings.User}:{_rabbitMqSettings.Password}")));

            var response =
                await client.GetAsync($"http://{_rabbitMqSettings.Host}:15672/api/exchanges/%2f/{exchangeName}");

            return response.IsSuccessStatusCode;
        }
    }

    private void InitializeRabbitMq()
    {
        _logger.WriteLog($"Initializing RabbitMQ, exchange bound to queue: {_queue}");
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

            // create queue if it doesnt exists and
            // bind exchange to queue
            channel.QueueDeclare(
                queue: _queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: args
            );
        }
    }

    // Performs a direct exchange publish
    // doesnt throw exceptions, writes logs when err ocurrs, returns boolean
    public override async Task<bool> PublishAsync<T>(T message, string exchange, string routingKey)
    {
        try
        {
            using (var channel = _rabbitConnection.CreateModel())
            {
                if (!await ExchangeExists(exchange))
                {
                    // create the exchange if doesnt exsits
                    try
                    {
                        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Error creating exchange: {exchange}");
                        _logger.LogRabbitMqError(_queue, ex.Message);
                        throw new CouldNotCreateExchangeException($"Message not posted to RabbitMQ, could not create exchange {exchange}");
                    }
                }

                // binds exchange to queue 
                channel.QueueBind(queue: _queue, exchange: exchange, routingKey: routingKey);

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

                Console.WriteLine(
                    $"--> Just published {message} to RabbitMQ exchange: {exchange} with routing key: {routingKey} on queue {_queue}");
                _logger.LogRabbitMqMessage(_queue, exchange, routingKey.ToString(), jsonMessage);
                return true;
            }
        }
        catch (CouldNotCreateExchangeException ex)
        {
            Console.WriteLine("--> Could not create exchange");
            _logger.LogRabbitMqError(_queue, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("--> Exception on RabbitMQ");
            _logger.LogRabbitMqError(_queue, ex.Message);
            return false;
        }
    }


    public override void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
    }
}
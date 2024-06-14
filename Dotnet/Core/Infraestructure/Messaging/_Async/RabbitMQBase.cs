using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Infraestructure.Messaging._Mail;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DemoLibrary.Infraestructure.Messaging.Async;

public abstract class RabbitMQBase : IAsyncMessagePublisher
{
    protected readonly string _queue;
    protected readonly IConnection _rabbitConnection;
    protected readonly IModel _channel;
    protected readonly RabbitMQSettings _rabbitMqSettings;

    public RabbitMQBase(
        IConnection rabbitConnection,
        string queue,
        IOptions<RabbitMQSettings> rabbitMqSettings)
    {
        // rabbitmq connection
        _rabbitConnection = rabbitConnection;

        // communication channel
        _channel = _rabbitConnection.CreateModel();

        // queue being heard
        _queue = queue;

        // rabbitmq creds
        _rabbitMqSettings = rabbitMqSettings.Value;
    }

    public abstract Task<bool> PublishAsync<T>(T message, string exchange, string routingKeys);
    public abstract void Dispose();
}
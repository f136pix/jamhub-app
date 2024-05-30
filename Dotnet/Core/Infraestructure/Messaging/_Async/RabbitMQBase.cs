using RabbitMQ.Client;

namespace DemoLibrary.Infraestructure.Messaging.Async;

public abstract class RabbitMQBase : IAsyncMessagePublisher
{
    protected readonly string _queue;
    protected readonly IConnection _rabbitConnection;
    protected readonly IModel _channel;

    public RabbitMQBase(
        IConnection rabbitConnection,
        string queue
    )
    {
        // rabbitmq connection
        _rabbitConnection = rabbitConnection;

        // communication channel
        _channel = _rabbitConnection.CreateModel();

        // queue being heard
        _queue = queue;
    }

    public abstract Task PublishAsync<T>(T message, string exchange, RoutingKeys routingKeys);
    public abstract void Dispose();
}
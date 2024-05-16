namespace DemoLibrary.Infraestructure.Messaging.Async;

public interface IAsyncMessagePublisher
{
    Task PublishAsync<T>(T message, string exchange, RoutingKeys routingKeys);
    
    void Dispose();
}
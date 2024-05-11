namespace DemoLibrary.Infraestructure.Messaging.Async;

public interface IAsyncMessagePublisher
{
    Task PublishAsync<T>(T message, RoutingKeys routingKeys);
    
    void Dispose();
}
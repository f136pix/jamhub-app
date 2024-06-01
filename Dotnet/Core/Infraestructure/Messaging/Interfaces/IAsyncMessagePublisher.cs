using DemoLibrary.Application.Services.Messaging;


public interface IAsyncMessagePublisher
{
    Task PublishAsync<T>(T message, string exchange, RoutingKeys routingKeys);
    
    void Dispose();
}
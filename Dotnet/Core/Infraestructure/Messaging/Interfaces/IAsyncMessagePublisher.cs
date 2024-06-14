using DemoLibrary.Application.Services.Messaging;


public interface IAsyncMessagePublisher
{
    Task<bool> PublishAsync<T>(T message, string exchange, string routingKeys);
    
    void Dispose();
}
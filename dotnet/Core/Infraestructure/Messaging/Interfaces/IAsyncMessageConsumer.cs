namespace DemoLibrary.Infraestructure.Messaging.Async;

public interface IAsyncMessageConsumer
{
    void InitializeConsumer();
    void Dispose(); 
}
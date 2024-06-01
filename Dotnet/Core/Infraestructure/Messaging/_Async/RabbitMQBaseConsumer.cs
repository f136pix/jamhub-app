using System.Text;
using DemoLibrary.CrossCutting.Logger;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DemoLibrary.Infraestructure.Messaging.Async;

// class only consumes messages
public abstract class RabbitMqBaseConsumer : IAsyncMessageConsumer
{
    protected readonly string Queue;
    protected readonly IConnection RabbitConnection;
    protected readonly IModel Channel;
    protected RabbitMqLogger Logger;


    public RabbitMqBaseConsumer(IConnection rabbitConnection, string queue)
    {
        RabbitConnection = rabbitConnection;
        Channel = RabbitConnection.CreateModel();
        Queue = queue;
        Logger = new RabbitMqLogger(logDirectory: "AMPQ");
    }

    int _currentRetry = 0;
    int _maxRetries = 3;

    // public void InitializeConsumer()
    // {
    //     if (_currentRetry < _maxRetries)
    //     {
    //         try
    //         {
    //             var consumer = new EventingBasicConsumer(Channel);
    //
    //             Console.WriteLine("Consumer initialized");
    //             // fired once a message is received
    //             consumer.Received += (model, ea) =>
    //             {
    //                 var body = ea.Body.ToArray();
    //                 var message = Encoding.UTF8.GetString(body);
    //                 HandleMessage(message, ea.RoutingKey.ToString());
    //                 Logger.LogConsumerRecievedMessage(Queue, ea.RoutingKey.ToString(), message);
    //             };
    //
    //             // acknowledge the message, so the channel can receive the next message
    //             Channel.BasicConsume(queue: Queue, autoAck: true, consumer: consumer);
    //
    //             _currentRetry = 0;
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"There was a error in the rabbitMQ consumer: {ex.Message}");
    //             // starting the consumer again
    //             InitializeConsumer();
    //             _currentRetry++;
    //             Console.WriteLine(_currentRetry);
    //         }
    //
    //         // if maximum retries breaks out of recursion
    //         if (_currentRetry >= _maxRetries)
    //         {
    //             Logger.WriteException($"RabbitMQ Consumer crashed after failing {_maxRetries} attemps");
    //             Console.WriteLine("Failed to initialize the consumer after maximum retries.");
    //         }
    //     }
    // }

    public void InitializeConsumer()
    {
        var consumer = new EventingBasicConsumer(Channel);

        Console.WriteLine("Consumer initialized");
        // fired once a message is received
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            HandleMessage(message, ea.RoutingKey.ToString());
            Logger.LogConsumerRecievedMessage(Queue, ea.RoutingKey.ToString(), message);
        };

        // acknowledge the message, so the channel can receive the next message
        Channel.BasicConsume(queue: Queue, autoAck: true, consumer: consumer);
    }


    protected abstract void HandleMessage(string message, string routingKey);

    public void Dispose()
    {
        Channel?.Dispose();
        RabbitConnection?.Dispose();
    }
}
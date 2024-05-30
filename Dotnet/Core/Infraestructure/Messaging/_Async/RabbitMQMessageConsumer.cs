using System;
using DemoLibrary.CrossCutting.Logger;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace DemoLibrary.Infraestructure.Messaging.Async;

// handles message processing logic
public class RabbitMqMessageConsumer : RabbitMqBaseConsumer
{
    public RabbitMqMessageConsumer(IConnection rabbitConnection, string queue)
        : base(rabbitConnection, queue)
    {
        
        //InitializeConsumer();
    }

    protected override void HandleMessage(string message, string routingKey)
    {
        Console.WriteLine("ola");
        // deserialize the message to T generic type and process it
        var deserializedMessage = JsonConvert.DeserializeObject<object>(message);
        Console.WriteLine($"--> Deserialized message = {deserializedMessage}");
        Console.WriteLine(deserializedMessage);
    }
}
using System;
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.CrossCutting.Logger;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace DemoLibrary.Infraestructure.Messaging.Async;

// handles message processing logic
public class RabbitMqMessageConsumer : RabbitMqBaseConsumer
{
    private readonly IAsyncProcessorService _asyncProcessorService;
    public RabbitMqMessageConsumer(IConnection rabbitConnection, string queue, IAsyncProcessorService asyncProcessorService)
        : base(rabbitConnection, queue)
    {
        _asyncProcessorService = asyncProcessorService;
        //InitializeConsumer();
    }

    protected override async Task HandleMessageAsync(string message, string routingKey)
    {
        // deserialize the json string to dotnet object 
        var deserializedMessage = JsonConvert.DeserializeObject<object>(message);
        Console.WriteLine($"--> Deserialized message = {deserializedMessage}");
        
        await _asyncProcessorService.ProcessMessage(message, routingKey);
    }
}
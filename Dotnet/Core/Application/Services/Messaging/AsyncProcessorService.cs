using AutoMapper;
using DemoLibrary.Application.CQRS.Messaging;
using DemoLibrary.Application.Dtos.Messaging;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DemoLibrary.Application.Services.Messaging;

public interface IAsyncProcessorService
{
    public Task ProcessMessage(object message, string routingKey);
}

public class AsyncProcessorService : IAsyncProcessorService
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, Func<object, Task>> _handlers;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly RabbitMqLogger _logger;

    public AsyncProcessorService(IMediator mediator, IMapper mapper, IConfiguration configuration)
    {
        _mediator = mediator;
        _mapper = mapper;
        // relates the routing key to the fucntion being called
        _handlers = new Dictionary<string, Func<object, Task>>
        {
            //{Acess key, Function to call}
            { "user.registered", HandleUserRegistered },
            // { nameof(RoutingKeys.UpdateUser), HandleUpdateUser },
            // { nameof(RoutingKeys.DeleteUser), HandleDeleteUser }
        };
        _configuration = configuration;

        _logger = new RabbitMqLogger(logDirectory: "AMPQ");
    }

    public async Task ProcessMessage(object message, string routingKey)
    {
        Console.WriteLine(message);
        try
        {
            await _handlers[routingKey](message);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"--> No handler found for routing key: {routingKey}");
            _logger.WriteException("No handler found for routing key :" + routingKey);
        }
        catch (SendEmailException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Error processing message: {ex.Message}");
        }
    }

    private async Task HandleUserRegistered(object message)
    {
        Console.WriteLine("--> Fell in handleRegisteredUser");
        var messageData = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfirmationEmailDataDto>(message.ToString()!);

        var subject = "Confirm your email at Jamhub";

        var hostUrl = _configuration["Host:Url"];
        var confirmationLink = $"{hostUrl}/confirm/{messageData!.ConfirmationToken}";
        Console.WriteLine("--> CONFIRMATION LINK: " + confirmationLink);

        var text = $@"
            <html>
                <body>
                    <h1>Confirm your Email</h1>
                    <p>Please click the button below to confirm your email:</p>
                    <a href='{confirmationLink}' target='_blank'>
                        <button>Confirm Email</button>
                    </a>
                </body>
            </html>";


        var sendEmailCommand = new SendEmailCommand("Filipecocinel@gmail.com", subject, text);
        await _mediator.Send(sendEmailCommand);
    }
}
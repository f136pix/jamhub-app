using AutoMapper;
using DemoLibrary.Application.CQRS.Blacklist;
using DemoLibrary.Application.CQRS.Messaging;
using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Application.Dtos.Messaging;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.Application.Services.Messaging;

public interface IAsyncProcessorService
{
    public Task ProcessMessage(object message, string routingKey);
}

public class AsyncProcessorService : IAsyncProcessorService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMediator _mediator;
    private readonly Dictionary<string, Func<object, Task>> _handlers;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly RabbitMqLogger _logger;

    public AsyncProcessorService(
        IMediator mediator,
        IMapper mapper,
        IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _mediator = mediator;
        _mapper = mapper;
        // relates the routing key to the fucntion being called
        _handlers = new Dictionary<string, Func<object, Task>>
        {
            //{Acess key, Function to call}
            { "user.registered", HandleUserRegistered },
            { "token.blacklisted", HandleBlacklistedToken },
            // { nameof(RoutingKeys.UpdateUser), HandleUpdateUser },
            // { nameof(RoutingKeys.DeleteUser), HandleDeleteUser }
        };
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
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


        // needs using a scope since 
        // AsyncProcessorService is a singleton
        // Mediator is transient, so are it dependencies
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var sendEmailCommand = new SendEmailCommand("Filipecocinel@gmail.com", subject, text);
            await mediator.Send(sendEmailCommand);

            var createPersonDto = new CreatePersonDto
            {
                Id = messageData.Id,
                Email = messageData.Email,
                ConfirmationToken = messageData.ConfirmationToken
            };

            // saves user in dotnet db as well
            var createPersonCommand = new CreatePersonCommand(createPersonDto);
            await mediator.Send(createPersonCommand);
        }

        // // var sendEmailCommand = new SendEmailCommand(messageData.Email, subject, text);
        // // sends confirmation email to user
        // var sendEmailCommand = new SendEmailCommand("Filipecocinel@gmail.com", subject, text);
        //
        // await _mediator.Send(sendEmailCommand);
        //
        // var createPersonDto = new CreatePersonDto
        // {
        //     Id = messageData.Id,
        //     Email = messageData.Email
        // };
        //
        // // saves user in dotnet db as well
        // var createPersonCommand = new CreatePersonCommand(createPersonDto);
        //
        // await _mediator.Send(createPersonCommand);
    }

    private async Task HandleBlacklistedToken(object message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            Console.WriteLine("--> Fell in handleBlacklistedToken");
            var createBlacklistedTokenDto =
                Newtonsoft.Json.JsonConvert.DeserializeObject<CreateBlacklistedTokenDto>(message.ToString()!);

            Console.WriteLine(createBlacklistedTokenDto.Jti);
            Console.WriteLine(createBlacklistedTokenDto.ExpiryDate);
            
             var createBlacklistedTokenCommand = new BlacklistCommand.CreateBlacklistCommand(createBlacklistedTokenDto!);
             await mediator.Send(createBlacklistedTokenCommand);
        }
    }
}
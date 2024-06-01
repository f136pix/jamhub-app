using MediatR;

namespace DemoLibrary.Application.Services.Messaging;

public interface IAsyncProcessorService
{
    public void ProcessMessage(object message, string routingKey);
}

public class AsyncProcessorService : IAsyncProcessorService
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, Action<object>> _handlers;

    public AsyncProcessorService(IMediator mediator)
    {
        _mediator = mediator;
        // relates the routing key to the fucntion being called
        _handlers = new Dictionary<string, Action<object>>
        {
            //{Acess key, Function to call}
            { nameof(RoutingKeys.CreateUser), HandleCreateUser },
            { "user.registered", HandleUserRegistered },
            // { nameof(RoutingKeys.UpdateUser), HandleUpdateUser },
            // { nameof(RoutingKeys.DeleteUser), HandleDeleteUser }
        };
    }

    public void ProcessMessage(object message, string routingKey)
    {
        Console.WriteLine(message);
        _handlers[routingKey](message);
    }

    private void HandleCreateUser(object message)
    {
        //var command = JsonConvert.DeserializeObject<CreateUserCommand>(message);
        //_mediator.Send(command);
    }

    private void HandleUserRegistered(object message)
    {
        Console.Write(message);
        //_mediator.Send(command);
    }
}
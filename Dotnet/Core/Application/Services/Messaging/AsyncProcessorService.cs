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
            { nameof(RoutingKeys.CreateUser), HandleCreateUser },
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
}
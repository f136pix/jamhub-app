using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Application.Services.People;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Infraestructure.Messaging.Async;
using MediatR;
using Newtonsoft.Json;

namespace DemoLibrary.Application.CQRS.Messaging;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private IAsyncMessagePublisher _asyncMessagePublisher;
    private IPeopleService _peopleService;

    public ConfirmEmailCommandHandler(RabbitMqMessagePublisher messagePublisher, IPeopleService peopleService)
    {
        _peopleService = peopleService;
        _asyncMessagePublisher = messagePublisher;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        
        // service has too much business logic and should be in a proper class
        await _peopleService.ConfirmUserAsync(request);
        
        // convert to json string json string
        var token = JsonConvert.SerializeObject(request);

        return await _asyncMessagePublisher.PublishAsync(token, "amq.direct", RoutingKeysConfig.ConfirmEmail);
    }
}
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Infraestructure.Messaging.Async;
using MediatR;
using Newtonsoft.Json;

namespace DemoLibrary.Application.CQRS.Messaging;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private IAsyncMessagePublisher _asyncMessagePublisher;

    public ConfirmEmailCommandHandler(RabbitMqMessagePublisher messagePublisher)
    {
        _asyncMessagePublisher = messagePublisher;
    }


    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        // make it json string
        var token = JsonConvert.SerializeObject(request);
        return await _asyncMessagePublisher.PublishAsync(token,  "amq.direct", RoutingKeysConfig.ConfirmEmail);
    }
}
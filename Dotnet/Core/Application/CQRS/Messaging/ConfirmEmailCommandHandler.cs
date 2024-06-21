using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.Messaging.Async;
using MediatR;
using Newtonsoft.Json;

namespace DemoLibrary.Application.CQRS.Messaging;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
{
    private readonly IAsyncMessagePublisher _asyncMessagePublisher;
    private readonly ITokenRepository<ConfirmationToken> _tokenRepository;
    private readonly IUnitOfWork _uow;
    // private IPeopleService _peopleService;

    public ConfirmEmailCommandHandler(RabbitMqMessagePublisher messagePublisher,
        ITokenRepository<ConfirmationToken> tokenRepository,
        IUnitOfWork uow)
    {
        _asyncMessagePublisher = messagePublisher;
        _tokenRepository = tokenRepository;
        _uow = uow;
    }

    public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        // await _peopleService.ConfirmUserAsync(request);

        var token = request.ConfirmationToken;

        var user = await _tokenRepository.GetByIdAsync(token)
            .ContinueWith(confirmationToken => confirmationToken.Result.User);

        if (user == null)
        {
            throw new CouldNotValidateUser("Invalid token");
        }

        if (user.Verified)
        {
            throw new CouldNotValidateUser("User already verified");
        }

        user.Verified = true;

        await _uow.CommitAsync();

        // convert to json string
        var jsonToken = JsonConvert.SerializeObject(request);

        return await _asyncMessagePublisher.PublishAsync(jsonToken, "amq.direct", RoutingKeysConfig.ConfirmEmail);
    }
}
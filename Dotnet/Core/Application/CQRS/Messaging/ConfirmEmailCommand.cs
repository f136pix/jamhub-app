using MediatR;

namespace DemoLibrary.Application.CQRS.Messaging;

public record ConfirmEmailCommand(string ConfirmationToken) : IRequest<bool>;
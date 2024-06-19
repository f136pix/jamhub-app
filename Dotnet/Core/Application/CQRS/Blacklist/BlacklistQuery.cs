using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

public record CheckBlacklistQuery(string jti) : IRequest<bool>;
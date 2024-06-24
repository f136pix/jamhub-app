using DemoLibrary.Application.DataAccess;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

public class BlacklistedTokenQueryHandler : IRequestHandler<CheckBlacklistQuery, bool>
{
    private readonly ITokenRepository<BlacklistedToken> _repository;

    public BlacklistedTokenQueryHandler(ITokenRepository<BlacklistedToken> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CheckBlacklistQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.jti);

        var blacklistToken = await _repository.GetByIdAsync(request.jti);

        Console.WriteLine("-->" + blacklistToken);

        return blacklistToken == null;
    }
}
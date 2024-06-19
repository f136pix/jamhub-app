using DemoLibrary.Infraestructure.DataAccess;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

public class BlacklistQueryHandler : IRequestHandler<CheckBlacklistQuery, bool>
{
    private readonly IBlacklistRepository _repository;

    public BlacklistQueryHandler(IBlacklistRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CheckBlacklistQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.jti);
        
        var blacklistToken = await _repository.GetBlacklistByJtiAsync(request.jti);
        
        Console.WriteLine("-->" + blacklistToken);

        return blacklistToken == null;
    }
}
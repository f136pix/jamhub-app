using AutoMapper;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using MediatR;


namespace DemoLibrary.Application.CQRS.Blacklist;

public class BlacklistedTokenCommandHandler : IRequestHandler<CreateBlacklistCommand, BlacklistedToken>
{
    private readonly ITokenRepository<BlacklistedToken> _blacklistRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public BlacklistedTokenCommandHandler(ITokenRepository<BlacklistedToken> blacklistRepository, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _blacklistRepository = blacklistRepository;
        _uow = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BlacklistedToken> Handle(CreateBlacklistCommand request,
        CancellationToken cancellationToken)
    {
        CreateBlacklistedTokenDto dto = request.dto;

        var jtiAlreadyBlacklisted = await _blacklistRepository.GetByIdAsync(dto.Jti);
        if (jtiAlreadyBlacklisted != null)
        {
            throw new AlreadyExistsException(dto.Jti);
        }

        var blacklist = new BlacklistedToken
        {
            Jti = dto.Jti,
            ExpiryDate = dto.ExpiryDate
        };

        await _blacklistRepository.AddAsync(blacklist);

        await _uow.CommitAsync();

        return blacklist;
    }
}
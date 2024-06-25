using AutoMapper;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using MediatR;


namespace DemoLibrary.Application.CQRS.Blacklist;

public class BlacklistedTokenCommandHandler :
    IRequestHandler<CreateBlacklistCommand, BlacklistedToken>,
    IRequestHandler<DeleteBlacklistCommand, bool>,
    // IRequestHandler<GetAllExpiredBlacklistCommand, List<BlacklistedToken>>
    IRequestHandler<DeleteExpiredBlacklistedCommand, List<BlacklistedToken>>
{
    private readonly ITokenRepository<BlacklistedToken> _blacklistRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly LoggerBase _logger;

    public BlacklistedTokenCommandHandler(ITokenRepository<BlacklistedToken> blacklistRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggerBaseFactory loggerFactory)
    {
        _blacklistRepository = blacklistRepository;
        _uow = unitOfWork;
        _mapper = mapper;
        _logger = loggerFactory.CreateRabbitMqLogger("BlacklistedToken-Handler");
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

    public async Task<bool> Handle(DeleteBlacklistCommand request, CancellationToken cancellationToken)
    {
        String token = request.token;

        var jtiAlreadyBlacklisted = await _blacklistRepository.GetByIdAsync(token);
        if (jtiAlreadyBlacklisted != null)
        {
            throw new NotFoundEntityException("BlacklistedToken", token);
        }

        await _blacklistRepository.DeleteAsync(token);

        await _uow.CommitAsync();

        return true;
    }


    public async Task<List<BlacklistedToken>> Handle(DeleteExpiredBlacklistedCommand request,
        CancellationToken cancellationToken)
    {
        List<BlacklistedToken> expiredTokens = await _blacklistRepository.GetAllExpiredAsync();
        
        List<BlacklistedToken> deletedTokens = new List<BlacklistedToken>();
        List<BlacklistedToken> errorTokens = new List<BlacklistedToken>();

        foreach (var token in expiredTokens)
        {
            try
            {
                await _blacklistRepository.DeleteAsync(token.Jti);
                deletedTokens.Add(token);
                _logger.WriteLog($"Deleted token {token.Jti}");
            }
            catch (Exception ex)
            {
                _logger.WriteException($"Could not delete token {token.Jti}, Error: {ex.Message}");
                errorTokens.Add(token);
            }
        }

        await _uow.CommitAsync();

        if (errorTokens.Count > 0)
            throw new DeleteExpiredBlacklistedTokensException(deletedTokens, errorTokens);

        return deletedTokens;
    }


    // public Task<List<BlacklistedToken>> Handle(GetAllExpiredBlacklistCommand request, CancellationToken cancellationToken)
    // {
    //     
    // }
}
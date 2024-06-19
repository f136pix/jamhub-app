using AutoMapper;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using MediatR;


namespace DemoLibrary.Application.CQRS.Blacklist;

public class BlacklistCommandHandler : IRequestHandler<CreateBlacklistCommand, Domain.Models.Blacklist>
{
    private readonly IBlacklistRepository _blacklistRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public BlacklistCommandHandler(IBlacklistRepository blacklistRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _blacklistRepository = blacklistRepository;
        _uow = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Domain.Models.Blacklist> Handle(CreateBlacklistCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.dto;

        var jtiAlreadyBlacklisted = await _blacklistRepository.GetBlacklistByJtiAsync(dto.Jti);
        if (jtiAlreadyBlacklisted != null)
        {
            throw new AlreadyExistsException(dto.Jti);
        }

        var blacklist = new Domain.Models.Blacklist
        {
            Jti = dto.Jti,
            ExpiryDate = dto.ExpiryDate
        };

        await _blacklistRepository.InsertBlacklistAsync(blacklist);

        await _uow.CommitAsync();

        return blacklist;
    }
}
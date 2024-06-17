using AutoMapper;
using DemoLibrary.Application.Dtos.Band;
using DemoLibrary.Application.Profiles;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Infraestructure.Messaging.Async;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public class BandCommandHandler :
    IRequestHandler<CreateBandCommand, Domain.Models.Band>
{
    private readonly IBandRepository _repository;
    private readonly IPeopleRepository _peopleRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private LoggerBase _logger;

    public BandCommandHandler(
        IBandRepository repository,
        IPeopleRepository peopleRepository,
        IUnitOfWork uow,
        IMapper mapper
    )
    {
        _repository = repository;
        _peopleRepository = peopleRepository;
        _uow = uow;
        _mapper = mapper;
        _logger = new LoggerBase("PeopleCommandHandler");
    }


    public async Task<Domain.Models.Band> Handle(CreateBandCommand request, CancellationToken cancellationToken)
    {
        // PersonCreateDto dto = request.dto;
        var dto = request.dto;

        _logger.WriteLog($"--> Creating band with name: {dto.Name}");

        var band = _mapper.Map<Domain.Models.Band>(dto);

        if (await _repository.isBandExistsAsync(dto.Name))
            throw new AlreadyExistsException("Name");

        if (await _peopleRepository.GetPersonByIdAsync(dto.CreatorId) == null)
        {
            throw new PersonNotFoundException(dto.CreatorId);
        }

        await _repository.InsertBandAsync(band);

        await _uow.CommitAsync();
        _logger.WriteLog($"--> Band with name {band.Name} created succefully!");

        return band;
    }
}
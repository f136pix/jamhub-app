using AutoMapper;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Dtos.Band;
using DemoLibrary.Application.Profiles;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Infraestructure.Messaging.Async;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public class BandCommandHandler :
    IRequestHandler<CreateBandCommand, Domain.Models.Band>
{
    private readonly ICommonRepository<Domain.Models.Band> _repository;
    private readonly ICommonRepository<Person> _peopleRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private LoggerBase _logger;

    public BandCommandHandler(
        ICommonRepository<Domain.Models.Band> repository,
        ICommonRepository<Person> peopleRepository,
        IUnitOfWork uow,
        IMapper mapper,
        ILoggerBaseFactory loggerFactory
    )
    {
        _repository = repository;
        _peopleRepository = peopleRepository;
        _uow = uow;
        _mapper = mapper;
        _logger = loggerFactory.Create("Create-Band");
    }


    public async Task<Domain.Models.Band> Handle(CreateBandCommand request, CancellationToken cancellationToken)
    {
        // PersonCreateDto dto = request.dto;
        var dto = request.dto;

        var band = _mapper.Map<Domain.Models.Band>(dto);

        if (await _repository.GetByProperty("Name", dto.Name) != null)
            throw new AlreadyExistsException("Name");

        if (await _peopleRepository.GetByIdAsync(dto.CreatorId) == null)
        {
            throw new PersonNotFoundException(dto.CreatorId);
        }

        await _repository.AddAsync(band);

        await _uow.CommitAsync();
        return band;
    }
}
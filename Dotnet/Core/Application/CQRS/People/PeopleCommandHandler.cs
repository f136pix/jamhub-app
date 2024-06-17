using System.Xml;
using AutoMapper;
using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Application.Services.People;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Infraestructure.Messaging.Async;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleCommandHandler :
    IRequestHandler<CreatePersonCommand, Person>,
    IRequestHandler<UpdatePersonCommand, Person>
{
    private readonly IPeopleService _peopleService;
    private readonly IConfirmationTokenRepository _confirmationTokenRepository;
    private readonly IBandRepository _bandRepository;
    private readonly IPeopleRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private IAsyncMessagePublisher _asyncMessagePublisher;
    private LoggerBase _logger;

    public PeopleCommandHandler(
        IPeopleService peopleService,
        IConfirmationTokenRepository confirmationTokenRepository,
        IBandRepository bandRepository,
        IPeopleRepository repository,
        IUnitOfWork uow,
        IMapper mapper,
        RabbitMqMessagePublisher asyncMessagePublisher
    )
    {
        _peopleService = peopleService;
        _confirmationTokenRepository = confirmationTokenRepository;
        _bandRepository = bandRepository;
        _repository = repository;
        _uow = uow;
        _mapper = mapper;
        _asyncMessagePublisher = asyncMessagePublisher;
        _logger = new LoggerBase("PeopleCommandHandler");
    }

    public async Task<Person> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // PersonCreateDto dto = request.dto;
            var dto = request.dto;

            _logger.WriteLog($"--> Creating person with email: {dto.Email}");
            if (await _repository.IsEmailExistsAsync(dto.Email))
                throw new AlreadyExistsException("Email");

            if (dto.Id.HasValue)
            {
                if (await _repository.GetPersonByIdAsync(dto.Id.Value) != null)
                {
                    throw new AlreadyExistsException("Id");
                }
            }

            return await _peopleService.RegisterUserAsync(dto);
        }
        catch (Exception ex)
        {
            _logger.WriteException($"Could not create user with email {request.dto.Email}, Error: {ex.Message}");
            throw;
        }
    }

    public async Task<Person> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        // PersonUpdateDto dto = request.dto;
        var dto = request.dto;

        var person = await _repository.GetPersonByIdAsync(dto.Id);

        if (person == null)
            throw new PersonNotFoundException(dto.Id);

        // person.FirstName = dto.FirstName;
        // person.LastName = dto.LastName;
        // person.Instrument = dto.Instrument;
        // person.CellphoneNumber = dto.CellphoneNumber;
        // person.CityName = dto.CityName;

        // updates our person
        _mapper.Map(dto, person);

        // ads the person to the recieved bands
        if (dto.BandIds != null)
        {
            foreach (var bandId in dto.BandIds)
            {
                await _bandRepository.GetBandByIdAsync(bandId).ContinueWith(task =>
                {
                    var band = task.Result;

                    if (band == null)
                    {
                        _logger.WriteLog($"--> Band with id {bandId} not found!");
                        throw new BandNotFoundException(bandId);
                    }

                    person.Bands.Add(band);
                });
            }
            // {
            //     person.Bands.Add(new BandModel { Name = band });
            // }
        }

        await _uow.CommitAsync();

        return person;
    }
}
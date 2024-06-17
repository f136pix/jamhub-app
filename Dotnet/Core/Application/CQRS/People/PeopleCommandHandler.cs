using System.Xml;
using AutoMapper;
using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Infraestructure.Messaging.Async;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleCommandHandler :
    IRequestHandler<CreatePersonCommand, PersonModel>,
    IRequestHandler<UpdatePersonCommand, PersonModel>
{
    private readonly IBandRepository _bandRepository;
    private readonly IPeopleRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private IAsyncMessagePublisher _asyncMessagePublisher;
    private LoggerBase _logger;

    public PeopleCommandHandler(
        IBandRepository bandRepository,
        IPeopleRepository repository,
        IUnitOfWork uow,
        IMapper mapper,
        RabbitMqMessagePublisher asyncMessagePublisher
    )
    {
        _bandRepository = bandRepository;
        _repository = repository;
        _uow = uow;
        _mapper = mapper;
        _asyncMessagePublisher = asyncMessagePublisher;
        _logger = new LoggerBase("PeopleCommandHandler");
    }

    public async Task<PersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        // PersonCreateDto dto = request.dto;
        var dto = request.dto;

        _logger.WriteLog($"--> Creating person with email: {dto.Email}");
        if (await _repository.IsEmailExistsAsync(dto.Email))
            throw new AlreadyExistsException("Email");

        var person = new PersonModel
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };
        await _repository.InsertPersonAsync(person);

        await _uow.CommitAsync();
        _logger.WriteLog($"--> Person with email {dto.Email} created succefully!");

        await _asyncMessagePublisher.PublishAsync(person, "users.create", RoutingKeysConfig.CreateUser);
        return person;
    }

    public async Task<PersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        // PersonUpdateDto dto = request.dto;
        var dto = request.dto;

        _logger.WriteLog($"--> Updating Person with id: {dto.Id}");

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
                        return;
                    }

                    person.Bands.Add(band);
                });
            }
            // {
            //     person.Bands.Add(new BandModel { Name = band });
            // }
        }

        await _uow.CommitAsync();
        _logger.WriteLog($"--> Person with id {dto.Id} updated succefully!");

        return person;
    }
}
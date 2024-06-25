using System.Xml;
using AutoMapper;
using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.Messaging.Async;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleCommandHandler :
    IRequestHandler<CreatePersonCommand, Person>,
    IRequestHandler<UpdatePersonCommand, Person>
{
    private readonly ITokenRepository<ConfirmationToken> _confirmationTokenRepository;
    private readonly ICommonRepository<Band> _bandRepository;
    private readonly ICommonRepository<Person> _repository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private IAsyncMessagePublisher _asyncMessagePublisher;
    private LoggerBase _logger;

    public PeopleCommandHandler(
        ITokenRepository<ConfirmationToken> confirmationTokenRepository,
        ICommonRepository<Band> bandRepository,
        ICommonRepository<Person> repository,
        IUnitOfWork uow,
        IMapper mapper,
        RabbitMqMessagePublisher asyncMessagePublisher,
        ILoggerBaseFactory loggerFactory
    )
    {
        _confirmationTokenRepository = confirmationTokenRepository;
        _bandRepository = bandRepository;
        _repository = repository;
        _uow = uow;
        _mapper = mapper;
        _asyncMessagePublisher = asyncMessagePublisher;
        _logger = loggerFactory.Create("People-Handler");
    }

    public async Task<Person> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            CreatePersonDto dto = request.dto;
            if (await _repository.GetByProperty("Email", dto.Email) != null)
                throw new AlreadyExistsException("Email");

            if (dto.Id.HasValue && await _repository.GetByIdAsync(dto.Id.Value) != null)
                throw new AlreadyExistsException("Id");

            if (dto.ConfirmationToken == null)
                throw new ArgumentNullException(nameof(ConfirmationToken.Token));

            Person person = _mapper.Map<Person>(dto);
            dto.Id = person.Id;

            ConfirmationToken confirmationToken = _mapper.Map<ConfirmationToken>(dto);

            await _repository.AddAsync(person);

            await _confirmationTokenRepository.AddAsync(confirmationToken);

            await _uow.CommitAsync();
            
            _logger.WriteLog($"Created user with email {request.dto.Email} successfully");

            return person;
        }
        catch (Exception ex)
        {
            _logger.WriteException($"Could not create user with email {request.dto.Email}, Error: {ex.Message}");
            throw;
        }
    }

    public async Task<Person> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        UpdatePersonDto dto = request.dto;

        var person = _mapper.Map<Person>(dto);
        var updatedPerson = await _repository.UpdateAsync(person);

        await _uow.CommitAsync();

        return updatedPerson;
    }
}
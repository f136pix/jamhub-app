using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Infraestructure.Messaging.Async;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleCommandHandler :
    IRequestHandler<CreatePersonCommand, PersonModel>
{
    private readonly IPeopleRepository _repository;
    private readonly IUnitOfWork _uow;
    private IAsyncMessagePublisher _asyncMessagePublisher;
    private LoggerBase _logger;

    public PeopleCommandHandler(IPeopleRepository repository, IUnitOfWork uow, RabbitMqMessagePublisher asyncMessagePublisher)
    {
        _repository = repository;
        _uow = uow;
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

        await _asyncMessagePublisher.PublishAsync(person, "users.create",RoutingKeys.CreateUser);
        return person;
    }
}
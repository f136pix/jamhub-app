using DemoLibrary.Business.Exceptions;
using DemoLibrary.CrossCutting.Logger;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleCommandHandler :
    IRequestHandler<CreatePersonCommand, PersonModel>
{
    private readonly IPeopleRepository _repository;
    private readonly IUnitOfWork _uow;
    private LoggerBase _logger;

    public PeopleCommandHandler(IPeopleRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
        _logger = new LoggerBase("PeopleCommandHandler");
    }

    public async Task<PersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        _logger.WriteLog($"--> Creating person with email: {request.Email}");
        if (await _repository.IsEmailExistsAsync(request.Email))
            throw new EmailAlreadyExistsException();

        var person = new PersonModel
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };
        await _repository.InsertPersonAsync(person);

        await _uow.CommitAsync();

        _logger.WriteLog($"--> Person with email {request.Email} created succefully!"); 
        return person;
    }
}
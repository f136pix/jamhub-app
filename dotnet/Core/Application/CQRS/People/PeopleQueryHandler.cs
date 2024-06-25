using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleQueryHandler :
    IRequestHandler<GetPeopleListQuery, IReadOnlyList<Person>>,
    IRequestHandler<GetPersonByIdQuery, Person>
{
    // private readonly IDataAccess _data;
    //
    // public PeopleHandler(IDataAccess data)
    // {
    //     _data = data;
    // }

    private readonly ICommonRepository<Person> _repository;

    public PeopleQueryHandler(ICommonRepository<Person> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<Person>> Handle(GetPeopleListQuery request, CancellationToken cancellationToken)
    {
        var people = await _repository.GetAllAsync();
        return people;
    }

    public async Task<Person> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.id);
        // Console.WriteLine("--> ",user.Bands.Count);

        return user;
    }
}
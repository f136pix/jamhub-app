using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleQueryHandler :
    IRequestHandler<GetPeopleListQuery, List<Person>>,
    IRequestHandler<GetPersonByIdQuery, Person>
{
    // private readonly IDataAccess _data;
    //
    // public PeopleHandler(IDataAccess data)
    // {
    //     _data = data;
    // }

    private readonly IPeopleRepository _repository;
    
    public PeopleQueryHandler(IPeopleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Person>> Handle(GetPeopleListQuery request, CancellationToken cancellationToken)
    {
        var people = await _repository.GetPeopleAsync();
        return people;
    }

public async Task<Person> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var user =  await _repository.GetPersonByIdAsync(request.id);
        // Console.WriteLine("--> ",user.Bands.Count);
        
        return user;
    }
}
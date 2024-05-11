using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public class PeopleQueryHandler :
    IRequestHandler<GetPeopleListQuery, List<PersonModel>>,
    IRequestHandler<GetPersonByIdQuery, PersonModel>
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

    public async Task<List<PersonModel>> Handle(GetPeopleListQuery request, CancellationToken cancellationToken)
    {
        var people = await _repository.GetPeopleAsync();
        return people;
    }

    public async Task<PersonModel> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var user =  await _repository.GetPersonByIdAsync(request.id);
        return user;
    }
}
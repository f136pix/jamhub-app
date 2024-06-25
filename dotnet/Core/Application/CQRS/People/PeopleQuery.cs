using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.People;

public record GetPeopleListQuery() :  IRequest<List<Person>>;

// public class GetPersonListQuery() : IRequest<List<PersonModel>>
// {
//     
// }

public record GetPersonByIdQuery(long id) : IRequest<Person>;



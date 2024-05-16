using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public record GetPeopleListQuery() :  IRequest<List<PersonModel>>;

// public class GetPersonListQuery() : IRequest<List<PersonModel>>
// {
//     
// }

public record GetPersonByIdQuery(int id) : IRequest<PersonModel>;



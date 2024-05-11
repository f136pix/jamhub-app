using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Business.CQRS.People;

public record CreatePersonCommand(string FirstName, string LastName, string Email) : IRequest<PersonModel>;

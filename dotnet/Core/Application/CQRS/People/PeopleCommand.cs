using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.People;

public record CreatePersonCommand(CreatePersonDto dto) : IRequest<Person>;

public record UpdatePersonCommand(UpdatePersonDto dto) : IRequest<Person>;


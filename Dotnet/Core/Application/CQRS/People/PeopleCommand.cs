using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.People;

public record CreatePersonCommand(CreatePersonDto dto) : IRequest<PersonModel>;


public record UpdatePersonCommand(UpdatePersonDto dto) : IRequest<PersonModel>;

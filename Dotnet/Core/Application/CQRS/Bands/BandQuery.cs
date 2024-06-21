using DemoLibrary.Domain.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public record GetBandListQuery() : IRequest<IReadOnlyList<Domain.Models.Band>>;

public record GetBandByIdQuery(int id) : IRequest<Domain.Models.Band>;
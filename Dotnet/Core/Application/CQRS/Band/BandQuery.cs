using DemoLibrary.Domain.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public record GetBandListQuery() : IRequest<List<BandModel>>;

public record GetBandByIdQuery(int id) : IRequest<BandModel>;
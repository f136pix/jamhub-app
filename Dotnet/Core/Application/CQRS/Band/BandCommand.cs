using DemoLibrary.Application.Dtos.Band;
using DemoLibrary.Domain.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public record CreateBandCommand(CreateBandDto dto) : IRequest<BandModel>;
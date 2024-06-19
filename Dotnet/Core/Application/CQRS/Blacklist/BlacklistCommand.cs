using DemoLibrary.Application.Dtos.Blacklist;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

    public record CreateBlacklistCommand(CreateBlacklistDto dto) : IRequest<Domain.Models.Blacklist>;
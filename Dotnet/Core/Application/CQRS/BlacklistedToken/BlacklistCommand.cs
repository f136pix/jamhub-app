using DemoLibrary.Application.Dtos.Blacklist;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

    public record CreateBlacklistCommand(CreateBlacklistedTokenDto dto) : IRequest<Domain.Models.BlacklistedToken>;
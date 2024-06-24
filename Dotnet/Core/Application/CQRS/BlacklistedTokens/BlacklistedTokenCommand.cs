using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Domain.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

    public record CreateBlacklistCommand(CreateBlacklistedTokenDto dto) : IRequest<BlacklistedToken>;
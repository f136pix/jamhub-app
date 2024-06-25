using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Domain.Models;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

    public record CreateBlacklistCommand(CreateBlacklistedTokenDto dto) : IRequest<BlacklistedToken>;
    
    public record DeleteBlacklistCommand(string token) : IRequest<bool>;

    // public record GetAllExpiredBlacklistCommand() : IRequest<List<BlacklistedToken>>;

    public record DeleteExpiredBlacklistedCommand() : IRequest<List<BlacklistedToken>>;
    
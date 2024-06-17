using DemoLibrary.Application.Dtos.Blacklist;
using MediatR;

namespace DemoLibrary.Application.CQRS.Blacklist;

public class BlacklistCommand
{
    public record CreateBlacklistCommand(CreateBlacklistedTokenDto dto) : IRequest<Domain.Models.Blacklist>;
}
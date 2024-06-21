using AutoMapper;
using DemoLibrary.Application.CQRS.Blacklist;
using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Domain.Models;

namespace DemoLibrary.Application.Profiles;

public class BlacklistProfile : Profile
{
    public BlacklistProfile()
    {
        // source  -> target
        // source : entry value 
        // target : mapped/ formated value
        CreateMap<CreateBlacklistCommand, BlacklistedToken>();
    }
}
using AutoMapper;
using DemoLibrary.Application.Dtos.Blacklist;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Domain.Models;
using DemoLibrary.Models;

namespace DemoLibrary.Application.Profiles;

public class ConfirmationTokenProfile : Profile
{
    public ConfirmationTokenProfile()
    {
        CreateMap<CreatePersonDto, ConfirmationToken>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.ConfirmationToken))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}
    
    

using AutoMapper;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Application.Resolvers;
using DemoLibrary.Domain.Models;
using DemoLibrary.Models;

namespace DemoLibrary.Application.Profiles;

public class PeopleProfile : Profile
{
    public PeopleProfile()
    {
        // source  -> target
        // source : entry value 
        // target : mapped/ formated value

        // removes null properties from source
        CreateMap<UpdatePersonDto, Person>()
            // .ForMember(p => p.Bands, opt => opt.MapFrom(x => x.BandsIds.Select(id => new Band { Id = id })))
            .ForMember(p => p.Bands, opt => opt.MapFrom<BandsIdsResolver>())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        CreateMap<CreatePersonDto, Person>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
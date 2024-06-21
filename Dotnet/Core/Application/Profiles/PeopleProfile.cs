using AutoMapper;
using DemoLibrary.Application.Dtos.People;
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
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CreatePersonDto, Person>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
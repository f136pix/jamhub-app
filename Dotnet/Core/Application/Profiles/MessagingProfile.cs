using AutoMapper;
using DemoLibrary.Application.Dtos.Messaging;

namespace DemoLibrary.Application.Profiles;

public class MessagingProfile : Profile
{
    public MessagingProfile()
    {
        // source  -> target
        // source : entry value 
        // target : mapped/ formated value
        CreateMap<object, SendEmailDto>();
    }
}
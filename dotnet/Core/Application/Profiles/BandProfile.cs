using AutoMapper;
using DemoLibrary.Application.Dtos.Band;
using DemoLibrary.Domain.Models;

namespace DemoLibrary.Application.Profiles;

public class BandProfile : Profile
{
        public BandProfile()
        {
            // source  -> target
            // source : entry value 
            // target : mapped/ formated value
            CreateMap<CreateBandDto, Band>();
        } 
}
using AutoMapper;
using DemoLibrary.Application.Dtos.Band;
using DemoLibrary.Domain.Models;

namespace DemoLibrary.Application.Profiles;

public class BandsProfile : Profile
{
        public BandsProfile()
        {
            // source  -> target
            // source : entry value 
            // target : mapped/ formated value
            CreateMap<CreateBandDto, BandModel>();
        } 
}
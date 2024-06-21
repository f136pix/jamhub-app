using AutoMapper;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Domain.Models;
using DemoLibrary.Models;

namespace DemoLibrary.Application.Resolvers;

public class BandsIdsResolver : IValueResolver<UpdatePersonDto, Person, ICollection<Band>>
{
    private ICommonRepository<Band> _repository;
    
    public BandsIdsResolver(ICommonRepository<Band> repository)
    {
        _repository = repository;
    } 
    
    public ICollection<Band> Resolve(UpdatePersonDto source, Person destination, ICollection<Band> destMember, ResolutionContext context)
    {
        if (source.BandsIds == null)
        {
            return destMember ?? new List<Band>(); // Return existing bands or empty list
        }
    
        var bands = new List<Band>();  
    
        foreach (int bandId in source.BandsIds)  
        {
            var band =  _repository.GetByIdSync(bandId);  
            if (band != null)
            {
                bands.Add(band);
            }
        }
    
        return bands;
    }
}
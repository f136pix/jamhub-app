using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public class BandQueryHandler :
    IRequestHandler<GetBandListQuery, List<Domain.Models.Band>>,
    IRequestHandler<GetBandByIdQuery, Domain.Models.Band>
{
    private readonly IBandRepository _repository;

    public BandQueryHandler(IBandRepository repository)
    {
        _repository = repository;
    }


    public async Task<List<Domain.Models.Band>> Handle(GetBandListQuery request, CancellationToken cancellationToken)
    {
        var bands = await _repository.GetBandsAsync();
        return bands;
    }

    public async Task<Domain.Models.Band> Handle(GetBandByIdQuery request, CancellationToken cancellationToken)
    {
        var band = await _repository.GetBandByIdAsync(request.id);
        // Console.WriteLine("--> ",user.Bands.Count);
        return band;
    }
}
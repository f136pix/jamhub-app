using DemoLibrary.Application.DataAccess;
using MediatR;

namespace DemoLibrary.Application.CQRS.Band;

public class BandQueryHandler :
    IRequestHandler<GetBandListQuery, IReadOnlyList<Domain.Models.Band>>,
    IRequestHandler<GetBandByIdQuery, Domain.Models.Band>
{
    private readonly ICommonRepository<Domain.Models.Band> _repository;

    public BandQueryHandler(ICommonRepository<Domain.Models.Band> repository)
    {
        _repository = repository;
    }


    public async Task<IReadOnlyList<Domain.Models.Band>> Handle(GetBandListQuery request,
        CancellationToken cancellationToken)
    {
        var bands = await _repository.GetAllAsync();
        return bands;
    }

    public async Task<Domain.Models.Band> Handle(GetBandByIdQuery request, CancellationToken cancellationToken)
    {
        var band = await _repository.GetByIdAsync(request.id);
        return band;
    }
}
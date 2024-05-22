using Api.Controllers.WebApi;
using DemoLibrary.Application.CQRS.Band;
using DemoLibrary.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BandsController : WebApiMediatorController
{
    public BandsController(IMediator mediator) : base(mediator)
    {
    }

    // GET: api/bands
    [HttpGet]
    public Task<ActionResult<List<BandModel>>> Get()
    {
        return HandleRequest<GetBandListQuery, List<BandModel>>(() => new GetBandListQuery());
    }

    // GET: api/bands/5
    [HttpGet("{id}")]
    public Task<ActionResult<BandModel>> Get(int id)
    {
        return HandleRequest<GetBandByIdQuery, BandModel>(() => new GetBandByIdQuery(id));
    }

    // POST: api/bands
    [HttpPost]
    public Task<ActionResult<BandModel>> Post([FromBody] CreateBandCommand command)
    {
        return HandleRequest<CreateBandCommand, BandModel>(() => command);
    }
}
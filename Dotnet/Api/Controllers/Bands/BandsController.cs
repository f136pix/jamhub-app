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
    public Task<ActionResult<List<Band>>> Get()
    {
        return HandleRequest<GetBandListQuery, List<Band>>(() => new GetBandListQuery());
    }

    // GET: api/bands/5
    [HttpGet("{id}")]
    public Task<ActionResult<Band>> Get(int id)
    {
        return HandleRequest<GetBandByIdQuery, Band>(() => new GetBandByIdQuery(id));
    }

    // POST: api/bands
    [HttpPost]
    public Task<ActionResult<Band>> Post([FromBody] CreateBandCommand command)
    {
        return HandleRequest<CreateBandCommand, Band>(() => command);
    }
}
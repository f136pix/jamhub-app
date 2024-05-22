using Api.Controllers.WebApi;
using DemoLibrary.Application.CQRS.People;
using DemoLibrary.Business.CQRS.People;
using DemoLibrary.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : WebApiMediatorController
{
    public PeopleController(IMediator mediator) : base(mediator)
    {
    }

    // GET: api/people
    [HttpGet]
    public Task<ActionResult<List<PersonModel>>> Get()
    {
        return HandleRequest<GetPeopleListQuery, List<PersonModel>>(() => new GetPeopleListQuery());
    }

    // GET: api/people/5
    [HttpGet("{id}")]
    public Task<ActionResult<PersonModel>> Get(int id)
    {
        return HandleRequest<GetPersonByIdQuery, PersonModel>(() => new GetPersonByIdQuery(id));
    }
    
    // POST: api/people
    [HttpPost]
    public Task<ActionResult<PersonModel>> Post([FromBody] CreatePersonCommand command)
    {
        return HandleRequest<CreatePersonCommand, PersonModel>(() => command);
    }
}
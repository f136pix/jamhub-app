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
    public Task<ActionResult<List<Person>>> Get()
    {
        return HandleRequest<GetPeopleListQuery, List<Person>>(() => new GetPeopleListQuery());
    }

    // GET: api/people/5
    [HttpGet("{id}")]
    public Task<ActionResult<Person>> Get(int id)
    {
        return HandleRequest<GetPersonByIdQuery, Person>(() => new GetPersonByIdQuery(id));
    }

    // user creations must be made through Rails API
    // POST: api/people
    // [HttpPost]
    // public Task<ActionResult<PersonModel>> Post([FromBody] CreatePersonCommand command)
    // {
    //     return HandleRequest<CreatePersonCommand, PersonModel>(() => command);
    // }

    // PUT: api/people/5
    [HttpPut("{id}")]
    public Task<ActionResult<Person>> Put(int id, [FromBody] UpdatePersonCommand command)
    {
        command.dto.Id = id;

        return HandleRequest<UpdatePersonCommand, Person>(() => command);
    }
}
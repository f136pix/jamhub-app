using DemoLibrary.Business.Exceptions;
using DemoLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.WebApi;

public partial class WebApiMediatorController : BaseApiMediator
{
    protected readonly IMediator _mediator;
    

    public WebApiMediatorController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }
   
    protected override async Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(Func<TRequest> requestFunc)
    {
        if (requestFunc == null)
        {
            return BadRequest("Request cannot be null");
        }

        var request = requestFunc.Invoke();

        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }

        try
        {
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
        catch (AlreadyExistsException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (CommitFailedException ex)
        {
            return Problem(ex.Message);
        }
        catch (PersonNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            return Problem(ex.Message);
        }
    }
}
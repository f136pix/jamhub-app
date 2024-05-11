using DemoLibrary.Business.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.WebApi;

public class WebApiMediatorController : ControllerBase
{
    protected readonly IMediator _mediator;

    public WebApiMediatorController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected async Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(Func<TRequest> requestFunc)
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
        catch (EmailAlreadyExistsException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (CommitFailedException ex)
        {
            return Problem(ex.Message);
        }
    }
}
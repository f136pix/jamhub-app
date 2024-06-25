using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.WebApi;

public abstract class BaseApiMediator : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseApiMediator(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected abstract Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(Func<TRequest> requestFunc);
    
    protected abstract Task<ContentResult> HandleRequestHTML<TRequest>(Func<TRequest> requestFunc);
}
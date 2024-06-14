using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Api.Controllers.WebApi;

public partial class WebApiMediatorController : BaseApiMediator
{
    protected readonly string _okMsg;
    protected readonly string _errMsg;
    protected readonly string _notFoundMsg;
    protected readonly string _unprocessableEntityMsg;

    public WebApiMediatorController(IMediator mediator, string okMsg, string errMsg,
        string notFoundMsg, string unprocessableEntityMsg) : base(mediator)
    {
        _mediator = mediator;
        if (string.IsNullOrEmpty(okMsg) || string.IsNullOrEmpty(errMsg))
        {
            throw new ArgumentNullException("Ok msg and Error msg cannot be null or empty");
        }

        _okMsg = okMsg;
        _errMsg = errMsg;
        _notFoundMsg = string.IsNullOrEmpty(notFoundMsg) ? errMsg : notFoundMsg;
        _unprocessableEntityMsg = string.IsNullOrEmpty(unprocessableEntityMsg) ? errMsg : unprocessableEntityMsg;
    }

    protected override async Task<ContentResult> HandleRequestHTML<TRequest>(Func<TRequest> requestFunc)
    {
        if (requestFunc == null)
        {
            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = 400,
                Content = _errMsg
            };
            ;
        }

        var request = requestFunc.Invoke();

        if (request == null)
        {
            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = 400,
                Content = _errMsg
            };
        }

        try
        {
            var response = await _mediator.Send(request);

            if (response == null)
            {
                return new ContentResult()
                {
                    ContentType = "text/html",
                    StatusCode = 404,
                    Content = _notFoundMsg
                };
            }

            if (response is bool b && b == false)
            {
                return new ContentResult()
                {
                    ContentType = "text/html",
                    StatusCode = 400,
                    Content = _errMsg
                };
            }

            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = _okMsg
            };
        }
        catch (Exception ex)
        {
            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = 400,
                Content = _errMsg
            };
        }
    }
}
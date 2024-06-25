using DemoLibrary.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Api.Controllers.WebApi;

public partial class WebApiMediatorController : BaseApiMediator
{
    protected string _okMsg;
    protected string _errMsg;
    protected string _notFoundMsg;
    protected string _unprocessableEntityMsg;

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
        catch (CouldNotValidateUser ex)
        {
            _errMsg = @"
                    <html>
                        <head>
                            <title>Could not confirm email</title>
                        </head>
                    <body>
                        <h1>Your user might already be validated</h1>
                            <p>Try accessing it again, try again later or contact-us.</p>
                    </body>
                    </html>";
            return new ContentResult()
            {
                ContentType = "text/html",
                StatusCode = 400,
                Content = _errMsg
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
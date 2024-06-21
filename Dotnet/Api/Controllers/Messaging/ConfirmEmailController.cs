using Api.Controllers.WebApi;
using DemoLibrary.Application.CQRS.Messaging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Api.Controllers;

[ApiController]
[Route("confirm")]
public class ConfirmEmailController : WebApiMediatorController
{
    private new static readonly string OkMsg = @"
<html>
    <head>
        <title>Account Confirmed</title>
    </head>
    <body>
        <h1>Account confirmed succefully</h1>
        <p>You might need waiting some minutes before accessing your account.</p>
    </body>
</html>";

    private new static readonly string ErrMsg = @"
<html>
    <head>
        <title>An Err ocurred</title>
    </head>
    <body>
        <h1>Could not confirm your account</h1>
        <p>Try again latter or contact us for more info.</p>
    </body>
</html>";


    public ConfirmEmailController(IMediator mediator) : base(mediator, OkMsg, ErrMsg, null, null)
    {
    }

    // GET: confirm/email/{confirmation_token}
    [HttpGet("{confirmation_token}")]
    public async Task<ContentResult> Get(string confirmation_token)
    {
        return await HandleRequestHTML<ConfirmEmailCommand>(() => new ConfirmEmailCommand(confirmation_token));
    }
}
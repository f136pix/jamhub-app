using System.Threading;
using System.Threading.Tasks;
using DemoLibrary.Application.CQRS.Messaging;
using DemoLibrary.Application.Dtos.Messaging;
using DemoLibrary.Infraestructure.Messaging.Interfaces;
using MediatR;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
{
    IEmailSender _emailSender;

    public SendEmailCommandHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        var sendEmailDto = new SendEmailDto
        {
            Email = request.Email,
            Subject = request.Subject,
            Message = request.Message
        };
        await _emailSender.SendEmailAsync(sendEmailDto);
        // _emailSender.SendEmailAsync();
    }
}
using DemoLibrary.Application.Dtos.Messaging;
using MediatR;

namespace DemoLibrary.Application.CQRS.Messaging;

public class SendEmailCommand : IRequest
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }

    public SendEmailCommand(SendEmailDto dto)
    {
        Email = dto.Email;
        Subject = dto.Subject;
        Message = dto.Message;
    }
    
    public SendEmailCommand(string email, string subject, string message)
    {
        Email = email;
        Subject = subject;
        Message = message;
    }
}
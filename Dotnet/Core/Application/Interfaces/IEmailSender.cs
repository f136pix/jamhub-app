using DemoLibrary.Application.Dtos.Messaging;

namespace DemoLibrary.Infraestructure.Messaging.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(SendEmailDto dto);
}
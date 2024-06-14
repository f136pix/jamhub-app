using System.Net;
using System.Net.Mail;
using DemoLibrary.Application.Dtos.Messaging;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Infraestructure.Messaging.Interfaces;
using Microsoft.Extensions.Options;

namespace DemoLibrary.Infraestructure.Messaging._Mail;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public Task SendEmailAsync(SendEmailDto dto)
    {
        Execute(dto).Wait();

        return Task.FromResult(0);
    }

    private async Task Execute(SendEmailDto data)
    {
        // var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        // Console.WriteLine(jsonData);
        // return;

        var fromEmail = _emailSettings.FromAddress;
        var toEmail = data.Email;

        Console.WriteLine(data.Email);

        MailMessage mail = new MailMessage
        {
            From = new MailAddress(fromEmail)
        };

        mail.To.Add(new MailAddress(toEmail));

        mail.Subject = data.Subject;
        mail.Body = data.Message;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.Normal;

        using SmtpClient smtp = new SmtpClient(_emailSettings.ServerAddress, _emailSettings.ServerPort);
        smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
        smtp.EnableSsl = _emailSettings.ServerUseSsl;
        // throws error if mail no sent
        try
        {
            await smtp.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new SendEmailException(ex.Message);
        }

        Console.WriteLine("Email sent successfully!");
    }
}
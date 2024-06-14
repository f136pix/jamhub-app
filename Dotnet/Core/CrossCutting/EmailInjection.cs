using DemoLibrary.Infraestructure.Messaging._Mail;
using DemoLibrary.Infraestructure.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class EmailInjection
{
    public static IServiceCollection AddEmailService(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }
}
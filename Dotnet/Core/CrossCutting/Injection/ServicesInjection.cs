using DemoLibrary.Application.Services.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class ServicesInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IAsyncProcessorService, AsyncProcessorService>();

        return services;
    }
}
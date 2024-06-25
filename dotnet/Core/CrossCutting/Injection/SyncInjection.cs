using DemoLibrary.Infraestructure.Messaging._Sync;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class SyncInjection
{
    public static IServiceCollection AddHttpPublisher(this IServiceCollection services)
    {
        services.AddSingleton<IHttpPublisher, HttpPublisher>();
        return services;
    }
}
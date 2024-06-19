using DemoLibrary.Application.Services.Messaging;
using DemoLibrary.Application.Services.People;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class ServicesInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IAsyncProcessorService, AsyncProcessorService>();
        services.AddScoped<IPeopleService, PeopleService>(); 

        return services;
    }
}
using DemoLibrary.CrossCutting.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoLibrary.CrossCutting;

public static class LoggerInjection
{
    public static IServiceCollection AddLoggerFactory(this IServiceCollection services)
    {
        services.AddTransient<ILoggerBaseFactory, LoggerBaseFactory>();
        return services;
    }
}
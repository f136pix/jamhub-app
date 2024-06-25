using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class TaskInjection
{
    public static IServiceCollection RunAsyncTasks(this IServiceCollection services)
    {
        services.AddHostedService<DeleteExpiredTokensTask>();
        return services;
    }
    
}
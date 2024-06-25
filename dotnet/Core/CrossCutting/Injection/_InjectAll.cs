using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class _InjectAll
{
    public static IServiceCollection InjectAll(this IServiceCollection services, IApplicationBuilder app)
    {
        return services;
    }
}
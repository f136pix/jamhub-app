using DemoLibrary.Infraestructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DemoLibrary.CrossCutting;

public static class MiddlewareInjection
{
    public static IApplicationBuilder AddJwtMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtMiddleware>();
    }
}
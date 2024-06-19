using DemoLibrary.Infraestructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace DemoLibrary.CrossCutting;

public static class ApplyMiddlewares
{
    public static IApplicationBuilder ApplyMiddleware(this IApplicationBuilder app)
    {
        app.AddJwtMiddleware();

        return app;
    }
}
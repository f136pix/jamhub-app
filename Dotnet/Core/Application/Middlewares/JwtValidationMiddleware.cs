using AutoMapper;
using DemoLibrary.Infraestructure.Messaging._Sync;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DemoLibrary.Application.Middlewares;

public class JwtValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpPublisher _httpPublisher;
    private readonly string _validationUrl;

    public JwtValidationMiddleware(RequestDelegate next, IHttpPublisher httpPublisher, IConfiguration configuration)
    {
        _next = next;
        _httpPublisher = httpPublisher;
        _validationUrl = configuration.GetSection("ValidationUrl").Value!;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            context.Response.StatusCode = 401; // not authorized
            return;
        }

        var jwt = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        Console.WriteLine("--> Token: ", jwt);

        var headers = new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {jwt}" }
        };

        var validationResponse = await _httpPublisher.GetAsync(_validationUrl, headers);
        // var validationResponse = await _client.GetAsync($"https://your-validation-url.com/validate?jwt={jwt}");

        Console.WriteLine("--> ", validationResponse);
        // if (validationResponse.IsSuccessStatusCode)
        // {
        //     await _next(context);
        // }
        if (false)
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = 401; // Unauthorized
        }
    }
}
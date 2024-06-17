using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DemoLibrary.Infraestructure.Middleware;

public class JwtMiddleware

{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;


    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }
        
        // extracts token from haeader
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


        if (token != null)
        {
            Console.WriteLine(token);
            await ValidateToken(context, token);
        }
        else
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized");
        }
    }


    private async Task ValidateToken(HttpContext context, string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false, // our devise jwt is not sending audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // ValidIssuer = _configuration["Jwt:Issuer"],
            // ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };

        try
        {
            var principal =
                new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var id = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            var email = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;

            // adds to http context
            context.Items["Id"] = id;
            context.Items["Email"] = email;

            await _next(context);
        }
        catch (SecurityTokenException ex)
        {
            Console.WriteLine(ex.Message);
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
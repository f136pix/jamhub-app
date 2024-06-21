using Microsoft.AspNetCore.Http;

namespace DemoLibrary.CrossCutting.Logger;

public interface ILoggerBaseFactory
{
    LoggerBase Create(string logDirectory);
    
    RabbitMqLogger CreateRabbitMqLogger(string logDirectory);

}

public class LoggerBaseFactory : ILoggerBaseFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggerBaseFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public LoggerBase Create(string logDirectory)
    {
        if (string.IsNullOrEmpty(logDirectory))
        {
            throw new ArgumentNullException(nameof(logDirectory));
        }

        return new LoggerBase(logDirectory, _httpContextAccessor);
    }

    public RabbitMqLogger CreateRabbitMqLogger(string logDirectory)
    {
        if (string.IsNullOrEmpty(logDirectory))
        {
            throw new ArgumentNullException(nameof(logDirectory));
        }

        return new RabbitMqLogger(logDirectory, _httpContextAccessor);
    }
}
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DemoLibrary.CrossCutting.Logger;

public class LoggerBase
{
    private ILogger _log;
    private readonly IHttpContextAccessor _httpContextAccessor;

    protected string? _divisor = new String('*', 50);
    protected string? _id;
    protected string? _email;
    protected string? _processId;

    public LoggerBase(string logDirectory, IHttpContextAccessor httpContextAccessor)
    {
        InitializeLogger(logDirectory);
        _httpContextAccessor = httpContextAccessor;

        _id = _httpContextAccessor.HttpContext?.Items["Id"]?.ToString() != null
            ? _httpContextAccessor.HttpContext?.Items["Id"]?.ToString()
            : "ServerRequest";
        _email = _httpContextAccessor.HttpContext?.Items["Email"]?.ToString() != null
            ? _httpContextAccessor.HttpContext?.Items["Email"]?.ToString()
            : "ServerRequest";
        _processId = _httpContextAccessor.HttpContext?.Items["ProcessId"]?.ToString() != null
            ? _httpContextAccessor.HttpContext?.Items["ProcessId"]?.ToString()
            : "ServerRequest";
        
        Console.Write($"----> LoggerBase: {_id} - {_email} - {_processId} \n");
    }

    private void InitializeLogger(string logDirectory)
    {
        try
        {
            _log = new LoggerConfiguration()
                .WriteTo.File($"tmp/logs/{logDirectory}/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public void WriteLog(string message)
    {
        _log.Information($"\n" +
                         $"{_divisor}\n" +
                         $"id:{_id}\n" +
                         $"proccess_id: {_processId}\n" +
                         $"{message}");
    }

    public void WriteException(string message)
    {
        _log.Error($"\n" +
                   $"{_divisor}\n" +
                   $"id:{_id}\n" +
                   $"proccess_id: {_processId}\n" +
                   $"{message}");
    }
}
using Serilog;

namespace DemoLibrary.CrossCutting.Logger;

public class LoggerBase
{
    private ILogger _log;

    public LoggerBase(string logDirectory)
    {
        InitializeLogger(logDirectory);
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
        _log.Information(message);
    }

    public void WriteException(string message)
    {
        _log.Error(message);
    }
}
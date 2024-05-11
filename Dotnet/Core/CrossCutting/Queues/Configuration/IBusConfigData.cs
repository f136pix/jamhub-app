namespace DemoLibrary.CrossCutting.Queues.Configuration;

public interface IBusConfigData
{
    string Host { get; set; }
    string User { get; set; }
    string Password { get; set; }
    string Port { get; set; }
    string VirtualHost { get; set; }
}
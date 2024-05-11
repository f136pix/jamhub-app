namespace DemoLibrary.CrossCutting.Queues.Configuration;

public class BusConfigData : IBusConfigData
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Port { get; set; }
    public string VirtualHost { get; set; }
}
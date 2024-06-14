namespace DemoLibrary.Infraestructure.Messaging._Mail;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Port { get; set; }
    public string VirtualHost { get; set; }
}

// "Host": "localhost",
// "User": "guest",
// "Password": "guest",
// "Port": "5672",
// "VirtualHost": "/"

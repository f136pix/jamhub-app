using Microsoft.AspNetCore.Http;

namespace DemoLibrary.CrossCutting.Logger
{
    public class RabbitMqLogger : LoggerBase
    {
        public RabbitMqLogger(string logDirectory, IHttpContextAccessor httpContextAccessor) : base(logDirectory,
            httpContextAccessor)
        {
        }

        public void LogRabbitMqMessage(string queue, string exchange, string routingKey, string message)
        {
            WriteLog(
                $"Published RabbitMQ Message: Queue = {queue}, Exchange = {exchange}, Routing Key = {routingKey} Message = {message}");
        }

        public void LogConsumerRecievedMessage(string queue, string routingKey, string message)
        {
            WriteLog(
                $"Consumer Recieved Message: Queue = {queue}, Routing Key = {routingKey} Message = {message}");
        }

        public void LogRabbitMqError(string queue, string error)
        {
            WriteException(
                $"RabbitMQ Error: Queue = {queue}, Error = {error}");
        }
    }
}
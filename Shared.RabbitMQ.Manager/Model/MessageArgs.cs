namespace Shared.RabbitMQ.Manager.Model;

public class MessageArgs
{
    public SendType SendType { get; set; }

    public string ExchangeName { get; set; }

    public string RabbitQueueName { get; set; }

    public string RouteName { get; set; }

}
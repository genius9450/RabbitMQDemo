namespace Shared.RabbitMQ.Manager.Model;

public class SubscribeArgs
{
    public string ExchangeName { get; set; }

    public string RabbitQueueName { get; set; }

    public string RouteName { get; set; }

    public SendType SendType { get; set; }

}
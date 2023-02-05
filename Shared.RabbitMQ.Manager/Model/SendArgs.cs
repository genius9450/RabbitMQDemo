namespace Shared.RabbitMQ.Manager.Model;

public class SendArgs<T> where T : class
{

    public string ExchangeName { get; set; }

    public string RouteKey { get; set; }

    public T SendData { get; set; }

}
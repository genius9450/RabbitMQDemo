namespace Shared.RabbitMQ.Manager.Model
{
    public class PushMessageArgs<T> where T: class
    {
        public SendType SendType { get; set; }

        public string ExchangeName { get; set; }

        public string RouteKey { get; set; }

        public T SendData { get; set; }

    }

    public enum SendType
    {
        direct = 1,
        fanout = 2,
        topic = 3,
    }
}

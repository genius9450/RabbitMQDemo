namespace Shared.RabbitMQ.Manager.Model
{
    public class PushMessageArgs<T> where T: class
    {
        public ExchangeType ExchangeType { get; set; }

        public string ExchangeName { get; set; }

        public string RouteKey { get; set; }

        public T SendData { get; set; }

    }

    public enum ExchangeType
    {
        Direct = 1,
        FanOut = 2,
        Topic = 3,
    }
}

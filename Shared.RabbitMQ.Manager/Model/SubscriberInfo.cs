namespace Shared.RabbitMQ.Manager.Model;

public class SubscriberInfo
{
    public string ExchangeName { get; set; }

    public string QueueName { get; set; }

    public string RouteName { get; set; }

    public ExchangeType ExchangeType { get; set; }

    public Mode Mode { get; set; } = Mode.Lock;

}

public enum Mode
{
    /// <summary>
    /// 排隊處理
    /// </summary>
    /// <remarks>
    /// 等待前次作業處理完畢後才繼續處理後續作業
    /// </remarks>
    Lock,

    /// <summary>
    /// 非同步處理
    /// </summary>
    /// <remarks>
    /// 使用非同步方式處理，無需等待前次作業完成之後才進行後續作業
    /// </remarks>
    Async
}
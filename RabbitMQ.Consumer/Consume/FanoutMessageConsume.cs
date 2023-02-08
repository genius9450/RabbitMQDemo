using Autofac;
using Shared.RabbitMQ.Manager.Interface;

namespace RabbitMQ.Consumer.Consume;

public class FanoutMessageConsume : IMessageConsume
{
    private readonly ILogger<FanoutMessageConsume> _logger;
    public FanoutMessageConsume(ILogger<FanoutMessageConsume> logger)
    {
        _logger = logger;
    }

    public async Task ConsumeAsync(string message)
    {
        await Task.Yield();
        _logger.LogInformation("Fanout 接收訊息: {message}", message);
    }
}
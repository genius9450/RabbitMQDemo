using Autofac;
using Shared.RabbitMQ.Manager.Interface;

namespace RabbitMQ.Consumer.Consume;

public class CommonMessageConsume : IMessageConsume
{
    private readonly ILogger<CommonMessageConsume> _logger;
    public CommonMessageConsume(ILogger<CommonMessageConsume> logger)
    {
        _logger = logger;
    }

    public async Task ConsumeAsync(string message)
    {
        await Task.Yield();
        _logger.LogInformation("接收訊息: {message}", message);
    }
}
using Autofac;
using EasyNetQ;
using Shared.RabbitMQ.Manager.Interface;

namespace RabbitMQ.Consumer.Consume;

public class CommonMessageConsumer : IMessageConsumer
{
    private readonly ILogger<CommonMessageConsumer> _logger;
    public CommonMessageConsumer(ILogger<CommonMessageConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task ConsumeAsync(string message, MessageProperties prop, MessageReceivedInfo info)
    {
        await Task.Yield();
        _logger.LogInformation("接收訊息: {message}", message);
        _logger.LogInformation("訊息屬性: {prop}", prop.ToString());
        _logger.LogInformation("訊息資訊: {message}", info.ToString());
    }
}
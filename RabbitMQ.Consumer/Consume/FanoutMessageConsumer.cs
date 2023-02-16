using Autofac;
using EasyNetQ;
using Shared.RabbitMQ.Manager.Interface;

namespace RabbitMQ.Consumer.Consume;

public class FanoutMessageConsumer : IMessageConsumer
{
    private readonly ILogger<FanoutMessageConsumer> _logger;
    private string _consumerId;

    public FanoutMessageConsumer(ILogger<FanoutMessageConsumer> logger)
    {
        _logger = logger;
        _consumerId = Guid.NewGuid().ToString();
    }
    
    public async Task ConsumeAsync(string message, MessageProperties prop, MessageReceivedInfo info)
    {

        _logger.LogInformation("{consumerId} 接收訊息(start): {message}, thread: {thread}", _consumerId, message, Thread.CurrentThread.ManagedThreadId);

        //await Task.Delay((new Random().Next(1, 10)) * 500);
        await Task.Delay(2000);

        _logger.LogInformation("{consumerId} 接收訊息(end): {message}, thread: {thread}", _consumerId, message, Thread.CurrentThread.ManagedThreadId);

    }
}
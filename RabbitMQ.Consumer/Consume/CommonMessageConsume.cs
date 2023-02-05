using Autofac;
using Shared.RabbitMQ.Manager.Interface;

namespace RabbitMQ.Consumer.Consume;

public class CommonMessageConsume : IMessageConsume
{
    private readonly ILogger<CommonMessageConsume> _logger;

    public CommonMessageConsume()
    {
        _logger = Consts.IocContainer.Resolve<ILogger<CommonMessageConsume>>(); ;
    }

    public void Consume(string message)
    {
        _logger.LogInformation("接收訊息: {message}", message);
    }


}
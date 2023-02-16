using EasyNetQ;

namespace Shared.RabbitMQ.Manager.Interface;

public interface IMessageConsumer
{
    Task ConsumeAsync(string message, MessageProperties prop, MessageReceivedInfo info);

}
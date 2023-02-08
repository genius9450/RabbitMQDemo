namespace Shared.RabbitMQ.Manager.Interface;

public interface IMessageConsume
{
    Task ConsumeAsync(string message);

}
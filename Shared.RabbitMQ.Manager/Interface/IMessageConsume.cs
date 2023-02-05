namespace Shared.RabbitMQ.Manager.Interface;

public interface IMessageConsume
{
    void Consume(string message);
}
using EasyNetQ;
using EasyNetQ.Topology;
using Shared.RabbitMQ.Manager.Extension;
using Shared.RabbitMQ.Manager.Interface;
using Shared.RabbitMQ.Manager.Model;

namespace Shared.RabbitMQ.Manager
{
    internal class SendMessageMange : ISend
    {
        public async Task SendMsgAsync(PushMessageArgs pushMsgArgs, IBus bus)
        {
            Message<object> message = new Message<object>(pushMsgArgs.SendMsg);
            var ex = RabbitMQManager.DeclareExchange(bus, pushMsgArgs.SendType, pushMsgArgs.ExchangeName);
            await bus.Advanced.PublishAsync<object>(ex, pushMsgArgs.RouteName.ToSafeString(), false, (IMessage<object>)message).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsCompleted || !task.IsFaulted);
            }));
        }

        public void SendMsg(PushMessageArgs pushMsgArgs, IBus bus)
        {
            Message<object> message = new Message<object>(pushMsgArgs.SendMsg);
            var exchange = RabbitMQManager.DeclareExchange(bus, pushMsgArgs.SendType, pushMsgArgs.ExchangeName);
            bus.Advanced.Publish<object>(exchange, pushMsgArgs.RouteName.ToSafeString(), false, (IMessage<object>)message);
        }
    }
}

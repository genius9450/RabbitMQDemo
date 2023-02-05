using EasyNetQ;
using Shared.RabbitMQ.Manager.Model;

namespace Shared.RabbitMQ.Manager.Interface;

internal interface ISend
{
    Task SendMsgAsync(PushMessageArgs pushMsgArgs, IBus bus);

    void SendMsg(PushMessageArgs pushMsgArgs, IBus bus);
}
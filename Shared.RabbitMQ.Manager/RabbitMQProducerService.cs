using EasyNetQ;
using EasyNetQ.Topology;
using Shared.RabbitMQ.Manager.Extension;
using Shared.RabbitMQ.Manager.Interface;
using Shared.RabbitMQ.Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ.Manager
{
    public class RabbitMQProducerService
    {
        private readonly IBus _bus;

        public RabbitMQProducerService(IBus bus) => this._bus = bus;

        /// <summary>
        /// 推播訊息
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        public void PushMessage<T>(PushMessageArgs<T> pushMessageArgs) where T : class
        {
            SendMsg(pushMessageArgs, this._bus);
        }

        /// <summary>
        /// 推播訊息(非同步)
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        /// <returns></returns>
        public async Task PushMessageAsync<T>(PushMessageArgs<T> pushMessageArgs) where T : class
        {
            await SendMsgAsync(pushMessageArgs, this._bus);
        }
        
        private async Task SendMsgAsync<T>(PushMessageArgs<T> pushMsgArgs, IBus bus) where T : class
        {
            var message = new Message<object>(pushMsgArgs.SendData);
            var ex = RabbitMQExtension.DeclareExchange(bus, pushMsgArgs.SendType, pushMsgArgs.ExchangeName);
            await bus.Advanced.PublishAsync<object>(ex, pushMsgArgs.RouteKey.ToSafeString(), false, (IMessage<object>)message).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsCompleted || !task.IsFaulted) ;
            }));
        }

        private void SendMsg<T>(PushMessageArgs<T> pushMsgArgs, IBus bus) where T : class
        {
            var message = new Message<object>(pushMsgArgs.SendData);
            var exchange = RabbitMQExtension.DeclareExchange(bus, pushMsgArgs.SendType, pushMsgArgs.ExchangeName);
            bus.Advanced.Publish<object>(exchange, pushMsgArgs.RouteKey.ToSafeString(), false, (IMessage<object>)message);
        }


    }
}

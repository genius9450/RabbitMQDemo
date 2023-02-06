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
        /// <param name="args"></param>
        public void PushMessage<T>(PushMessageArgs<T> args) where T : class
        {
            var message = new Message<object>(args.SendData);
            var exchange = _bus.DeclareExchange(args.SendType, args.ExchangeName);
            _bus.Advanced.Publish<object>(exchange, args.RouteKey.ToSafeString(), false, (IMessage<object>)message);
        }

        /// <summary>
        /// 推播訊息(非同步)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task PushMessageAsync<T>(PushMessageArgs<T> args) where T : class
        {
            var message = new Message<object>(args.SendData);
            var ex = _bus.DeclareExchange(args.SendType, args.ExchangeName);
            await _bus.Advanced.PublishAsync<object>(ex, args.RouteKey.ToSafeString(), false, (IMessage<object>)message).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsCompleted || !task.IsFaulted) ;
            }));
        }

    }
}

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
            var message = new Message<object>(pushMessageArgs.SendData);
            var exchange = _bus.Advanced.ExchangeDeclare(pushMessageArgs.ExchangeName, pushMessageArgs.SendType.ToString());
            _bus.Advanced.Publish<object>(exchange, pushMessageArgs.RouteKey.ToSafeString(), false, (IMessage<object>)message);
        }

        /// <summary>
        /// 推播訊息(非同步)
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        /// <returns></returns>
        public async Task PushMessageAsync<T>(PushMessageArgs<T> pushMessageArgs) where T : class
        {
            var message = new Message<object>(pushMessageArgs.SendData);
            var ex = await _bus.Advanced.ExchangeDeclareAsync(pushMessageArgs.ExchangeName, pushMessageArgs.SendType.ToString());
            await _bus.Advanced.PublishAsync<object>(ex, pushMessageArgs.RouteKey.ToSafeString(), false, (IMessage<object>)message).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsCompleted || !task.IsFaulted) ;
            }));
        }

    }
}

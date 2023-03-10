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
    public class RabbitMQConsumerService
    {

        private readonly IBus _bus;
        private static readonly object LockHelper = new();

        public RabbitMQConsumerService(IBus bus)
        {
            _bus = bus;
        }

        /// <summary>
        /// 訂閱
        /// </summary>
        /// <typeparam name="TConsume"></typeparam>
        /// <param name="args"></param>
        /// <param name="consume"></param>
        public void Subscribe<TConsume>(SubscribeArgs args, TConsume consume) where TConsume : IMessageConsume
        {
            if (string.IsNullOrEmpty(args.ExchangeName))
                return;

            var exchange = _bus.DeclareExchange(args.SendType, args.ExchangeName);
            var queue = string.IsNullOrEmpty(args.RabbitQueueName) ? _bus.Advanced.QueueDeclare() : _bus.Advanced.QueueDeclare(args.RabbitQueueName);

            _bus.Advanced.Bind(exchange, queue, args.RouteName.ToSafeString());
            _bus.Advanced.Consume(queue,   async (body, properties, info) =>
            {
                var lockTaken = false;
                try
                {
                    Monitor.Enter(LockHelper, ref lockTaken);
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    await consume.ConsumeAsync(message);
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(LockHelper);
                }
            });
        }
        
    }
}

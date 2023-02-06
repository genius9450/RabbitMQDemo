﻿using EasyNetQ;
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

        public RabbitMQConsumerService(IBus bus) => this._bus = bus;

        /// <summary>
        /// 訂閱
        /// </summary>
        /// <typeparam name="TConsume"></typeparam>
        /// <param name="args"></param>
        /// <param name="consume"></param>
        public void Subscribe<TConsume>(MessageArgs args, TConsume consume) where TConsume : IMessageConsume
        {
            if (string.IsNullOrEmpty(args.ExchangeName))
                return;

            var exchange = RabbitMQExtension.DeclareExchange(this._bus, args.SendType, args.ExchangeName);
            var queue = RabbitMQExtension.QueueDeclare(this._bus, args);

            this._bus.Advanced.Bind(exchange, queue, args.RouteName.ToSafeString());
            this._bus.Advanced.Consume(queue, (body, properties, info) =>
            {
                var lockTaken = false;
                try
                {
                    Monitor.Enter(LockHelper, ref lockTaken);
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    consume.Consume(message);
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(LockHelper);
                }
            });
        }

        //private Queue QueueDeclare(MessageArgs args) => string.IsNullOrEmpty(args.RabbitQueueName) ? this._bus.Advanced.QueueDeclare() : this._bus.Advanced.QueueDeclare(args.RabbitQueueName);

        //private static Exchange DeclareExchange(IBus bus, SendType sendType, string exchangeName)
        //{
        //    var exchange = new Exchange(); ;
        //    switch (sendType)
        //    {
        //        case SendType.Direct:
        //            exchange = bus.Advanced.ExchangeDeclare(exchangeName, "direct");
        //            break;
        //        case SendType.Fanout:
        //            exchange = bus.Advanced.ExchangeDeclare(exchangeName, "fanout");
        //            break;
        //        case SendType.Topic:
        //            exchange = bus.Advanced.ExchangeDeclare(exchangeName, "topic");
        //            break;
        //    }
        //    return exchange;
        //}

    }
}

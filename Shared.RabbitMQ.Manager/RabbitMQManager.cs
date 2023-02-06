using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Shared.RabbitMQ.Manager.Extension;
using Shared.RabbitMQ.Manager.Interface;
using Shared.RabbitMQ.Manager.Model;

namespace Shared.RabbitMQ.Manager
{
    public class RabbitMQManager
    {
        private readonly IBus _bus;
        private static readonly object LockHelper = new();

        public RabbitMQManager(IBus bus) => this._bus = bus;

        /// <summary>
        /// 推播訊息
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        public void PushMessage(PushMessageArgs pushMessageArgs) => new SendMessageMange().SendMsg(pushMessageArgs, this._bus);

        /// <summary>
        /// 推播訊息(非同步)
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        /// <returns></returns>
        public async Task PushMessageAsync(PushMessageArgs pushMessageArgs) => await new SendMessageMange().SendMsgAsync(pushMessageArgs, this._bus);

        /// <summary>
        /// 訂閱
        /// </summary>
        /// <typeparam name="TConsume"></typeparam>
        /// <param name="args"></param>
        //public void Subscribe<TConsume>(MessageArgs args) where TConsume : IMessageConsume, new()
        //{
        //    if (string.IsNullOrEmpty(args.ExchangeName))
        //        return;

        //    var exchange = RabbitMQManager.DeclareExchange(this._bus, args.SendType, args.ExchangeName);
        //    var queue = this.QueueDeclare(args);
        //    this._bus.Advanced.Bind(exchange, queue, args.RouteName.ToSafeString());
        //    Expression<Action<TConsume>> methodCall;
        //    this._bus.Advanced.Consume(queue, (body, properties, info) =>
        //    {
        //        var lockTaken = false;
        //        try
        //        {
        //            Monitor.Enter(LockHelper, ref lockTaken);
        //            var message = Encoding.UTF8.GetString(body.ToArray());
        //            methodCall = (Expression<Action<TConsume>>)(job => job.Consume(message));
        //            methodCall.Compile()(new TConsume());
        //        }
        //        finally
        //        {
        //            if (lockTaken)
        //                Monitor.Exit(LockHelper);
        //        }
        //    });
        //}

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

            var exchange = RabbitMQManager.DeclareExchange(this._bus, args.SendType, args.ExchangeName);
            var queue = this.QueueDeclare(args);
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

        /// <summary>
        /// 推播訊息(Direct) - 非同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendArgs"></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public async Task SendDirectAsync<T>(SendArgs<T> sendArgs) where T : class
        {
            var args = new PushMessageArgs()
            {
                SendMsg = (object)sendArgs.SendData,
                ExchangeName = sendArgs.ExchangeName,
                RouteName = sendArgs.RouteKey,
                SendType = SendType.Direct
            };
            await this.PushMessageAsync(args).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsFaulted)
                    throw new AggregateException($"SendMessage Faulted: Exception:{(object)task.Exception}");
            }));
        }

        /// <summary>
        /// 推播訊息(Fanout) - 非同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendArgs"></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public async Task SendFanoutAsync<T>(SendArgs<T> sendArgs) where T : class
        {
            var args = new PushMessageArgs()
            {
                SendMsg = (object)sendArgs.SendData,
                ExchangeName = sendArgs.ExchangeName,
                SendType = SendType.Fanout
            };
            await this.PushMessageAsync(args).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsFaulted)
                    throw new AggregateException($"SendMessage Faulted: Exception:{(object)task.Exception}");
            }));
        }

        /// <summary>
        /// 推播訊息(Topic) - 非同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendArgs"></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public async Task SendTopicAsync<T>(SendArgs<T> sendArgs) where T : class
        {
            var args = new PushMessageArgs()
            {
                SendMsg = (object)sendArgs.SendData,
                ExchangeName = sendArgs.ExchangeName,
                RouteName = sendArgs.RouteKey,
                SendType = SendType.Topic
            };
            await this.PushMessageAsync(args).ContinueWith((Action<Task>)(task =>
            {
                if (task.IsFaulted)
                    throw new AggregateException($"SendMessage Faulted: Exception:{(object)task.Exception}");
            }));
        }

        private Queue QueueDeclare(MessageArgs args) => string.IsNullOrEmpty(args.RabbitQueueName) ? this._bus.Advanced.QueueDeclare() : this._bus.Advanced.QueueDeclare(args.RabbitQueueName);

        internal static Exchange DeclareExchange(IBus bus, SendType sendType, string exchangeName)
        {
            var exchange = new Exchange(); ;
            switch (sendType)
            {
                case SendType.Direct:
                    exchange = bus.Advanced.ExchangeDeclare(exchangeName, "direct");
                    break;
                case SendType.Fanout:
                    exchange = bus.Advanced.ExchangeDeclare(exchangeName, "fanout");
                    break;
                case SendType.Topic:
                    exchange = bus.Advanced.ExchangeDeclare(exchangeName, "topic");
                    break;
            }
            return exchange;
        }

    }
}

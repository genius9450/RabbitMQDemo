using EasyNetQ;
using EasyNetQ.Topology;
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
        private static readonly object LockHelper = new();

        public RabbitMQProducerService(IBus bus) => this._bus = bus;

        /// <summary>
        /// 推播訊息
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        public void PushMessage(PushMessageArgs pushMessageArgs) => new SendMessageService().SendMsg(pushMessageArgs, this._bus);

        /// <summary>
        /// 推播訊息(非同步)
        /// </summary>
        /// <param name="pushMessageArgs"></param>
        /// <returns></returns>
        public async Task PushMessageAsync(PushMessageArgs pushMessageArgs) => await new SendMessageService().SendMsgAsync(pushMessageArgs, this._bus);

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

    }
}

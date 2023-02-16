using EasyNetQ;
using Shared.RabbitMQ.Manager.Extension;
using Shared.RabbitMQ.Manager.Interface;
using Shared.RabbitMQ.Manager.Model;
using System.Text;
using Autofac;
using Microsoft.Extensions.Logging;

namespace Shared.RabbitMQ.Manager
{
    public class RabbitMQConsumerService
    {
        private readonly IBus _bus;
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private readonly ILifetimeScope _lifetimeScope;
        private static readonly SemaphoreSlim Locker = new (1, 1);

        public RabbitMQConsumerService(IBus bus, ILogger<RabbitMQConsumerService> logger, ILifetimeScope lifetimeScope)
        {
            _bus = bus;
            _logger = logger;
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// 訂閱
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <param name="args"></param>
        /// <param name="consumer"></param>
        public void Subscribe<TConsumer>(SubscriberInfo args, TConsumer consumer) where TConsumer : IMessageConsumer
        {
            if (string.IsNullOrEmpty(args.ExchangeName))
                return;

            var exchange = _bus.DeclareExchange(args.ExchangeType, args.ExchangeName);
            var queue = string.IsNullOrEmpty(args.QueueName) ? _bus.Advanced.QueueDeclare() : _bus.Advanced.QueueDeclare(args.QueueName);
            _bus.Advanced.Bind(exchange, queue, args.RouteName.ToSafeString());

            switch (args.Mode)
            {
                case Mode.Lock:
                    _bus.Advanced.Consume(queue, (body, properties, info) => ProcessWithLockAsync(consumer, body, properties, info));
                    break;
                case Mode.Async:
                    _bus.Advanced.Consume(queue, (body, properties, info) => ProcessAsync(consumer, body, properties, info));
                    break;
            }
        }


        /// <summary>
        /// 訂閱
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <param name="args"></param>
        public void Subscribe<TConsumer>(SubscriberInfo args) where TConsumer : IMessageConsumer
        {
            if (string.IsNullOrEmpty(args.ExchangeName))
                return;

            var exchange = _bus.DeclareExchange(args.ExchangeType, args.ExchangeName);
            var queue = string.IsNullOrEmpty(args.QueueName) ? _bus.Advanced.QueueDeclare() : _bus.Advanced.QueueDeclare(args.QueueName);
            _bus.Advanced.Bind(exchange, queue, args.RouteName.ToSafeString());

            switch (args.Mode)
            {
                case Mode.Lock:
                    _bus.Advanced.Consume(queue, ProcessWithLockAsync<TConsumer>);
                    break;
                case Mode.Async:
                    _bus.Advanced.Consume(queue, ProcessAsync<TConsumer>);
                    break;
            }
        }

        private async Task ProcessAsync<TConsumer>(TConsumer consumer, ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info) where TConsumer : IMessageConsumer
        {
            try
            {
                var message = Encoding.UTF8.GetString(body.ToArray());
                await consumer.ConsumeAsync(message, properties, info);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consumer Error!");
            }
        }

        private async Task ProcessWithLockAsync<TConsumer>(TConsumer consumer, ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info) where TConsumer : IMessageConsumer
        {
            try
            {
                await Locker.WaitAsync();
                var message = Encoding.UTF8.GetString(body.ToArray());
                await consumer.ConsumeAsync(message, properties, info);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consumer Error!");
            }
            finally
            {
                Locker.Release();
            }
        }


        private async Task ProcessAsync<TConsumer>(ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info) where TConsumer : IMessageConsumer
        {
            var consume = default(TConsumer);
            try
            {
                var message = Encoding.UTF8.GetString(body.ToArray());
                consume = _lifetimeScope.Resolve<TConsumer>();
                await consume.ConsumeAsync(message, properties, info);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consumer Error!");
            }
            finally
            {
                if (consume == null || consume.Equals(default(TConsumer))) { }
                else
                {
                    //_lifetimeScope.Dispose();
                }
            }
        }

        private async Task ProcessWithLockAsync<TConsume>(ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info) where TConsume : IMessageConsumer
        {
            var consume = default(TConsume);
            try
            {
                await Locker.WaitAsync();
                var message = Encoding.UTF8.GetString(body.ToArray());
                consume = _lifetimeScope.Resolve<TConsume>();
                await consume.ConsumeAsync(message, properties, info);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Consumer Error!");
            }
            finally
            {
                if (consume == null || consume.Equals(default(TConsume))) { }
                else
                {
                    //_lifetimeScope.Dispose();
                }
                
                Locker.Release();
            }
        }
    }
}

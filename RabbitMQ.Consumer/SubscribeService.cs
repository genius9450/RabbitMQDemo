using RabbitMQ.Consumer.Consume;
using Shared.RabbitMQ.Manager;
using Shared.RabbitMQ.Manager.Model;

namespace RabbitMQ.Consumer
{
    public class SubscribeService
    {
        private readonly ILogger<SubscribeService> _logger;
        private readonly RabbitMQConsumerService _consumerService;
        private readonly CommonMessageConsumer _commonConsumer;
        private readonly FanoutMessageConsumer _fanoutConsumer;

        public SubscribeService( ILogger<SubscribeService> logger, RabbitMQConsumerService consumerService, CommonMessageConsumer commonConsumer, FanoutMessageConsumer fanoutConsumer)
        {
            _logger = logger;
            _consumerService = consumerService;
            _commonConsumer = commonConsumer;
            _fanoutConsumer = fanoutConsumer;
        }


        public void Subscribe()
        {
            _logger.LogInformation("SubscribeService Start");

            #region 訂閱Direct模式

            //var directArgs = new SubscriberInfo()
            //{
            //    ExchangeType = ExchangeType.Direct,
            //    ExchangeName = "demo.test.Direct",
            //    QueueName = "demo.test.Direct.queue",
            //    RouteName = "demo.1"
            //};
            //_consumerService.SubscribeWithLock<CommonMessageConsumer>(directArgs, _commonConsumer);

            #endregion

            #region 訂閱Fanout模式

            var fanoutArgs = new SubscriberInfo()
            {
                ExchangeType = ExchangeType.FanOut,
                ExchangeName = "demo.test.FanOut",
                QueueName = "demo.test.FanOut.queue",
                Mode = Mode.Lock
            };
            //_consumerService.SubscribeWithLock<FanoutMessageConsumer>(fanoutArgs, _fanoutConsumer);
            _consumerService.Subscribe<FanoutMessageConsumer>(fanoutArgs, _fanoutConsumer);
            //_consumerService.Subscribe<FanoutMessageConsumer>(fanoutArgs);

            //var fanoutArgs2 = new SubscriberInfo()
            //{
            //    ExchangeType = ExchangeType.FanOut,
            //    ExchangeName = "demo.test.FanOut",
            //    QueueName = "demo.test.FanOut.queue.2"
            //};
            //_consumerService.SubscribeWithLock<FanoutMessageConsumer>(fanoutArgs2, _fanoutConsumer);

            //var fanoutArgs = new SubscriberInfo()
            //{
            //    ExchangeType = ExchangeType.FanOut,
            //    ExchangeName = "webhook.message.exchange",
            //    QueueName = "webhook.message.messagecounter.queue"
            //};
            //_consumerService.SubscribeWithLock<FanoutMessageConsumer>(fanoutArgs, _fanoutConsumer);

            //var fanoutArgs2 = new SubscriberInfo()
            //{
            //    ExchangeType = ExchangeType.FanOut,
            //    ExchangeName = "webhook.message.exchange",
            //    QueueName = "webhook.message.messageautoreply.queue"
            //};
            //_consumerService.SubscribeWithLock<FanoutMessageConsumer>(fanoutArgs2, _fanoutConsumer);

            #endregion

            #region 訂閱Topic模式

            //var topicArgs = new SubscriberInfo()
            //{
            //    ExchangeType = ExchangeType.Topic,
            //    ExchangeName = "demo.test.Topic",
            //    QueueName = "demo.test.Topic.queue",
            //    RouteName = "demo.Topic.*"
            //};
            //_consumerService.SubscribeWithLock<CommonMessageConsumer>(topicArgs, _commonConsumer);

            #endregion

        }


    }
}

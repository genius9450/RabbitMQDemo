using RabbitMQ.Consumer.Consume;
using Shared.RabbitMQ.Manager;
using Shared.RabbitMQ.Manager.Model;

namespace RabbitMQ.Consumer
{
    public class SubscribeService
    {
        private readonly RabbitMQConsumerService _consumerService;
        private readonly CommonMessageConsume _commonConsume;
        private readonly FanoutMessageConsume _fanoutConsume;

        public SubscribeService(RabbitMQConsumerService consumerService, CommonMessageConsume commonConsume, FanoutMessageConsume fanoutConsume)
        {
            _consumerService = consumerService;
            _commonConsume = commonConsume;
            _fanoutConsume = fanoutConsume;
        }


        public void Subscribe()
        {

            #region 訂閱Direct模式

            var directArgs = new MessageArgs()
            {
                SendType = SendType.direct,
                ExchangeName = "demo.test.direct",
                RabbitQueueName = "demo.test.direct.queue",
                RouteName = "demo.1"
            };
            _consumerService.Subscribe<CommonMessageConsume>(directArgs, _commonConsume);

            #endregion

            #region 訂閱Fanout模式

            var fanoutArgs = new MessageArgs()
            {
                SendType = SendType.fanout,
                ExchangeName = "demo.test.fanout",
                RabbitQueueName = "demo.test.fanout.queue"
            };
            _consumerService.Subscribe<FanoutMessageConsume>(fanoutArgs, _fanoutConsume);

            var fanoutArgs2 = new MessageArgs()
            {
                SendType = SendType.fanout,
                ExchangeName = "demo.test.fanout",
                RabbitQueueName = "demo.test.fanout.queue.2"
            };
            _consumerService.Subscribe<FanoutMessageConsume>(fanoutArgs2, _fanoutConsume);

            #endregion

            #region 訂閱Topic模式

            var topicArgs = new MessageArgs()
            {
                SendType = SendType.topic,
                ExchangeName = "demo.test.topic",
                RabbitQueueName = "demo.test.topic.queue",
                RouteName = "demo.topic.*"
            };
            _consumerService.Subscribe<CommonMessageConsume>(topicArgs, _commonConsume);

            #endregion

        }


    }
}

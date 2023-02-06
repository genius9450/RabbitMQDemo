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
                SendType = SendType.Direct,
                ExchangeName = "demo.test.Direct",
                RabbitQueueName = "demo.test.Direct.queue",
                RouteName = "demo.1"
            };
            _consumerService.Subscribe<CommonMessageConsume>(directArgs, _commonConsume);

            #endregion

            #region 訂閱Fanout模式

            var fanoutArgs = new MessageArgs()
            {
                SendType = SendType.Fanout,
                ExchangeName = "demo.test.Fanout",
                RabbitQueueName = "demo.test.Fanout.queue"
            };
            _consumerService.Subscribe<FanoutMessageConsume>(fanoutArgs, _fanoutConsume);

            var fanoutArgs2 = new MessageArgs()
            {
                SendType = SendType.Fanout,
                ExchangeName = "demo.test.Fanout",
                RabbitQueueName = "demo.test.Fanout.queue.2"
            };
            _consumerService.Subscribe<FanoutMessageConsume>(fanoutArgs2, _fanoutConsume);

            #endregion

            #region 訂閱Topic模式

            var topicArgs = new MessageArgs()
            {
                SendType = SendType.Topic,
                ExchangeName = "demo.test.Topic",
                RabbitQueueName = "demo.test.Topic.queue",
                RouteName = "demo.Topic.*"
            };
            _consumerService.Subscribe<CommonMessageConsume>(topicArgs, _commonConsume);

            #endregion

        }


    }
}

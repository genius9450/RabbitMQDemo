using RabbitMQ.Consumer.Consume;
using Shared.RabbitMQ.Manager;
using Shared.RabbitMQ.Manager.Model;

namespace RabbitMQ.Consumer
{
    public class SubscribeService
    {
        private readonly RabbitMQManager _manager;
        private readonly CommonMessageConsume _commonConsume;

        public SubscribeService(RabbitMQManager manager, CommonMessageConsume commonConsume)
        {
            _manager = manager;
            _commonConsume = commonConsume;
        }


        public void Subscribe()
        {

            #region 訂閱Direct模式

            var directArgs = new MessageArgs()
            {
                SendType = SendType.Direct,
                ExchangeName = "demo.test.direct",
                RabbitQueueName = "demo.test.direct.queue",
                RouteName = "demo.1"
            };
            _manager.Subscribe<CommonMessageConsume>(directArgs, _commonConsume);

            #endregion

            #region 訂閱Fanout模式

            var fanoutArgs = new MessageArgs()
            {
                SendType = SendType.Fanout,
                ExchangeName = "demo.test.fanout",
                RabbitQueueName = "demo.test.fanout.queue"
            };
            _manager.Subscribe<CommonMessageConsume>(fanoutArgs, _commonConsume);

            var fanoutArgs2 = new MessageArgs()
            {
                SendType = SendType.Fanout,
                ExchangeName = "demo.test.fanout",
                RabbitQueueName = "demo.test.fanout.queue.2"
            };
            _manager.Subscribe<CommonMessageConsume>(fanoutArgs2, _commonConsume);

            #endregion

            #region 訂閱Topic模式

            var topicArgs = new MessageArgs()
            {
                SendType = SendType.Topic,
                ExchangeName = "demo.test.topic",
                RabbitQueueName = "demo.test.topic.queue",
                RouteName = "demo.topic.*"
            };
            _manager.Subscribe<CommonMessageConsume>(topicArgs, _commonConsume);

            #endregion

        }


    }
}

using EasyNetQ.Topology;
using EasyNetQ;
using Shared.RabbitMQ.Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ.Manager.Extension
{
    public static class RabbitMQExtension
    {
        public static string ToSafeString(this object obj) => obj == null ? "" : obj.ToString();

        public static Exchange DeclareExchange(this IBus bus, SendType sendType, string exchangeName)
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

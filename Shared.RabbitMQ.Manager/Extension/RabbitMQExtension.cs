using EasyNetQ.Topology;
using EasyNetQ;
using Shared.RabbitMQ.Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeType = Shared.RabbitMQ.Manager.Model.ExchangeType;

namespace Shared.RabbitMQ.Manager.Extension
{
    public static class RabbitMQExtension
    {
        public static string ToSafeString(this object obj) => obj == null ? "" : obj.ToString();

        public static Exchange DeclareExchange(this IBus bus, ExchangeType exchangeType, string exchangeName)
        {
            var exchange = new Exchange(); ;
            switch (exchangeType)
            {
                case ExchangeType.Direct:
                    exchange = bus.Advanced.ExchangeDeclare(exchangeName, "direct");
                    break;
                case ExchangeType.FanOut:
                    exchange = bus.Advanced.ExchangeDeclare(exchangeName, "fanout");
                    break;
                case ExchangeType.Topic:
                    exchange = bus.Advanced.ExchangeDeclare(exchangeName, "topic");
                    break;
            }
            return exchange;
        }

    }
}

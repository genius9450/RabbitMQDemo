using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMQ.Manager.Model
{
    public class PushMessageArgs
    {
        public object SendMsg { get; set; }

        public SendType SendType { get; set; }

        public string ExchangeName { get; set; }

        public string RouteName { get; set; }

    }

    public enum SendType
    {
        Direct = 1,
        Fanout = 2,
        Topic = 3,
    }
}

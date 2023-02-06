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
        
    }
}

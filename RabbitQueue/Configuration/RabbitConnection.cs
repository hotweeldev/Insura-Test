using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitQueue.Configuration
{
    public class RabbitConnection
    {
        public string hostName { get; set; }
        public ushort hostPort { get; set; }
        public string queueName { get; set; }
        public string virtualHost { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}

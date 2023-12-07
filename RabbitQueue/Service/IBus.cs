using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitQueue.Service
{
    public interface IBus
    {
        Task SendAsync<T>(string queue, T message);
        Task ReceiveAsync<T>(string queue, Action<T> onMessage);
    }
}

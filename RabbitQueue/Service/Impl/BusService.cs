using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace RabbitQueue.Service.Impl
{
    public class BusService : IBus
    {
        private readonly IModel channel;

        public BusService(IModel channel)
        {
            this.channel = channel;
        }

        public async Task ReceiveAsync<T>(string queue, Action<T> onMessage)
        {
            channel.QueueDeclare(queue, true, false, false);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (s, e) =>
            {
                var json = Encoding.UTF8.GetString(e.Body.Span);
                var item = JsonConvert.DeserializeObject<T>(json);
                onMessage(item);
                await Task.Yield();
            };

            channel.BasicConsume(queue, true, consumer);
            await Task.Yield();
        }

        public async Task SendAsync<T>(string queue, T message)
        {
            await Task.Run(() =>
            {
                channel.QueueDeclare(queue, true, false, false);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = false;

                var output = JsonConvert.SerializeObject(message);
                channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(output));
            });
        }
    }
}

using RabbitMQ.Client;
using RabbitQueue.Service;
using RabbitQueue.Service.Impl;

namespace RabbitQueue.Connection
{
    public class RabbitHutch
    {
        private static ConnectionFactory? connectionFactory;
        private static IConnection? connection;
        private static IModel? channel;

        public static IBus CreateBus(string hostName)
        {
            connectionFactory = new ConnectionFactory { HostName = hostName, DispatchConsumersAsync = true };
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

            return new BusService(channel);
        }

        public static IBus CreateBus(string hostName, ushort hostPort, string virtualHost, string username, string password)
        {
            connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = hostPort,
                VirtualHost = virtualHost,
                UserName = username,
                Password = password,
                DispatchConsumersAsync = true
            };

            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();

            return new BusService(channel);
        }
    }
}

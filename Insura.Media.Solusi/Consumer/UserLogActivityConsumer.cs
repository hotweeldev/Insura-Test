using AutoMapper;
using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Models;
using Insura.Media.Solusi.Repository;
using RabbitQueue.Configuration;
using RabbitQueue.Service;

namespace Insura.Media.Solusi.Consumer
{
    public class UserLogActivityConsumer : BackgroundService
    {
        private readonly RabbitConnection connection;
        private readonly DataContext db;
        private readonly IBus busControl;
        private readonly IMapper mapper;

        public UserLogActivityConsumer(RabbitConnection connection, DataContext db, IMapper mapper, IBus busControl)
        {
            this.connection = connection;
            this.db = db;
            this.mapper = mapper;
            this.busControl = busControl;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await busControl.ReceiveAsync<CreateLogActivityCommand>(connection.queueName, x =>
            {
                Task.Run(() =>
                {
                    CreateLogActivity(x);
                }, stoppingToken);
            });
        }

        private void CreateLogActivity(CreateLogActivityCommand activity)
        {
            var userActivity = mapper.Map<UserLogActivity>(activity);
            db.UserLogActivities.Add(userActivity);
            db.SaveChanges();
        }
    }
}

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Notifications.API.Dtos.InputModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notifications.API.Consumers
{
    public class UserUpdatedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string Queue = "notification-service/user-updated";
        private const string Exchange = "notification-service";

        public UserUpdatedConsumer(
            IServiceProvider serviceProvider,
            IConfiguration configuration
        )
        {
            _serviceProvider = serviceProvider;

            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "root",
                Password = "123456"
            };

            _connection = connectionFactory.CreateConnection(
                "notifications-service-consumer"
            );

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            _channel.QueueBind(
                queue: Queue,
                exchange: "user-service",
                routingKey: "user-updated"
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();

                var contentString = Encoding.UTF8.GetString(contentArray);

                var message = JsonConvert
                    .DeserializeObject<UserUpdatedInputModel>(
                        contentString
                    );

                // await SendEmail(message)

                Console.WriteLine(
                    $"Message UserUpdated receveid with Id {message.Id}"
                );

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(
                queue: Queue,
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }
}

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notifications.API.Consumers
{
    public class UserCreatedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string Queue = "notification-service/user-created";
        private const string Exchange = "notification-service";

        public UserCreatedConsumer(
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
            _channel.ExchangeDeclare(
                exchange: Exchange,
                type: ExchangeType.Topic,
                durable: true
            );
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
                routingKey: "user-created"
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();

                var contentString = Encoding.UTF8.GetString(contentArray);

                var message = JsonConvert.DeserializeObject<UserCreated>(
                    contentString
                );

                // await SendEmail(message)

                Console.WriteLine(
                    $"Message UserCreated receveid with Id {message.Id}"
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

    public class UserCreated
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

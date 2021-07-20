using RabbitMQ.Client;
using Users.Domain.Interfaces.MessageBus;

namespace Users.Infra.MessageBus
{
    public class RabbitMQClient : IMessageBusClient
    {
        private readonly IConnection _connection;

        public RabbitMQClient()
        {
            var connectionFactory = new ConnectionFactory {
                HostName = "localhost"
            };

            _connection = connectionFactory.CreateConnection(
                "users-service-producer"
            );
        }

        public void Publish(object message, string routingKey, string exchange)
        {
            //
        }
    }
}

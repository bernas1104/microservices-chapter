using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Notifications.API.Dtos.InputModels;

namespace Notifications.API.Consumers
{
    public class UserCreatedConsumer :
        AbstractConsumer<UserCreatedInputModel>
    {
        public UserCreatedConsumer(
            IServiceProvider serviceProvider,
            IConfiguration configuration
        ) : base(serviceProvider, configuration)
        {
            Queue = "notification-service/user-created";

            _routingKey = "user-created";

            _exchange = "user-service";

            InitializeEventBus();
        }

        protected override void LogMessageReceived(
            UserCreatedInputModel message
        )
        {
            Console.WriteLine(
                $"Message UserCreated received with Id {message.Id}"
            );
        }

        protected override async Task SendEmail(UserCreatedInputModel message)
        {
            await Task.Delay(5000);

            Console.WriteLine("E-mail enviado!");

            return;
        }
    }
}

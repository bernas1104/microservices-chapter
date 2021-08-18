using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Notifications.API.Dtos.InputModels;

namespace Notifications.API.Consumers
{
    public class UserUpdatedConsumer : AbstractConsumer<UserUpdatedInputModel>
    {
        public UserUpdatedConsumer(
            IServiceProvider serviceProvider,
            IConfiguration configuration
        ) : base(serviceProvider, configuration)
        {
            Queue = "notification-service/user-updated";

            _routingKey = "user-updated";

            _exchange = "user-service";

            InitializeEventBus();
        }

        protected override void LogMessageReceived(
            UserUpdatedInputModel message
        )
        {
            Console.WriteLine(
                $"Message UserUpdated received with Id {message.Id}"
            );
        }

        protected override async Task SendEmail(UserUpdatedInputModel message)
        {
            await Task.Delay(5000);

            Console.WriteLine("E-mail enviado para avisar sobre atualização!");
        }
    }
}

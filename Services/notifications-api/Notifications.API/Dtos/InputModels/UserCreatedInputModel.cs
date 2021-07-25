using System;

namespace Notifications.API.Dtos.InputModels
{
    public class UserCreatedInputModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System;

namespace Notifications.API.Dtos.InputModels
{
    public class UserUpdatedInputModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string NewEmail { get; set; }
        public string OldEmail { get; set; }
    }
}

using System;
using Users.Domain.Events.Interfaces;

namespace Users.Domain.Events
{
    public class UserUpdated : IDomainEvent
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string NewEmail { get; private set; }
        public string OldEmail { get; set; }

        public UserUpdated(
            Guid id,
            string fullName,
            string newEmail,
            string oldEmail
        )
        {
            Id = id;
            FullName = fullName;
            NewEmail = newEmail;
            OldEmail = oldEmail;
        }
    }
}

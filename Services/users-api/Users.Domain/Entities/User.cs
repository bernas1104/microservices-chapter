using System;
using Users.Domain.Events;

namespace Users.Domain.Entities
{
    public class User : AggregateRoot
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public User(Guid id, string fullName, string email)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            DeletedAt = null;
        }

        public static User Create(string fullName, string email)
        {
            var user = new User(Guid.NewGuid(), fullName, email);

            user.AddEvent(new UserCreated(user.Id, user.FullName, user.Email));

            return user;
        }

        public void Update(string email)
        {
            var oldEmail = Email;

            Email = email;

            AddEvent(new UserUpdated(Id, FullName, Email, oldEmail));
        }
    }
}

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
            // Validações
            // 1. É um e-mail válido?
            // 3. Etc...

            var user = new User(Guid.NewGuid(), fullName, email);

            user.AddEvent(new UserCreated(user.Id, user.FullName, user.Email));

            return user;
        }

        public void Update(string email)
        {
            // Validações sobre o e-mail.
            // 1. É um e-mail válido?
            // 2. E-mail novo é igual ao atual?
            // 3. Etc...

            var oldEmail = Email;

            Email = email;

            AddEvent(new UserUpdated(Id, FullName, Email, oldEmail));
        }
    }
}

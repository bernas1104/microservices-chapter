using System;
using System.Collections.Generic;
using Users.Domain.Events.Interfaces;

namespace Users.Domain.Entities
{
    public class AggregateRoot
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        public Guid Id { get; set; }
        public IEnumerable<IDomainEvent> Events => _events;

        protected void AddEvent(IDomainEvent @event)
        {
            _events.Add(@event);
        }
    }
}

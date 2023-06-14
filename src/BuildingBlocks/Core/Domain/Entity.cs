using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Core.Domain.Events;

namespace BuildingBlocks.Core.Domain
{
    public abstract class Entity<TId> : IHasDomainEvents
        where TId : struct
    {
        private readonly List<DomainEvent> _domainEvents = new();

        public TId Id { get; init; }
        
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}

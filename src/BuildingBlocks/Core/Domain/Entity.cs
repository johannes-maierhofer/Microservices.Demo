using System.ComponentModel.DataAnnotations.Schema;
using Argo.MD.BuildingBlocks.Core.Domain.Events;

namespace Argo.MD.BuildingBlocks.Core.Domain
{
    public abstract class Entity<TId> : IHasDomainEvents
        where TId : struct
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public TId Id { get; init; }
        
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}

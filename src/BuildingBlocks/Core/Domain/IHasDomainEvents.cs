using BuildingBlocks.Core.Domain.Events;

namespace BuildingBlocks.Core.Domain
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void AddDomainEvent(DomainEvent eventItem);
        void RemoveDomainEvent(DomainEvent eventItem);
        void ClearDomainEvents();
    }
}

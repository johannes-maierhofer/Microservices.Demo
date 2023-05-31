using BuildingBlocks.Core.Model.Events;

namespace BuildingBlocks.Core.Model
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void AddDomainEvent(DomainEvent eventItem);
        void RemoveDomainEvent(DomainEvent eventItem);
        void ClearDomainEvents();
    }
}

using Argo.MD.BuildingBlocks.Core.Domain.Events;

namespace Argo.MD.BuildingBlocks.Core.Domain
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void AddDomainEvent(DomainEvent eventItem);
        void RemoveDomainEvent(DomainEvent eventItem);
        void ClearDomainEvents();
    }
}

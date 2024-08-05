namespace Argo.MD.BuildingBlocks.Core.Domain.Events
{
    public interface IDomainEventPublisher
    {
        Task Publish(DomainEvent domainEvent);
    }
}

namespace BuildingBlocks.Core.Model.Events
{
    public interface IDomainEventPublisher
    {
        Task Publish(DomainEvent domainEvent);
    }
}

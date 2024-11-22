using MediatR;

namespace Argo.MD.BuildingBlocks.Core.Domain.Events
{
    public class DomainEventPublisher(IMediator mediator) : IDomainEventPublisher
    {
        public async Task Publish(IDomainEvent domainEvent)
        {
            await mediator.Publish(domainEvent);
        }
    }
}

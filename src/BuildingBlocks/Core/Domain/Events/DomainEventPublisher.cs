using MediatR;

namespace Argo.MD.BuildingBlocks.Core.Domain.Events
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMediator _mediator;

        public DomainEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}

using BuildingBlocks.Messaging;
using Customers.Domain.CustomerAggregate;
using MediatR;
using Customers.Messages.Events;

namespace Customers.Features.Customers.Events
{
    public class CustomerCreatedIntegrationEventPublishingHandler : INotificationHandler<CustomerCreatedEvent>
    {
        private readonly IMessageBus _messageBus;

        public CustomerCreatedIntegrationEventPublishingHandler(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task Handle(CustomerCreatedEvent createdEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new CustomerCreated(createdEvent.Customer.Id);
            await _messageBus.Publish(integrationEvent, cancellationToken);
        }
    }
}

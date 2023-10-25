using BuildingBlocks.Messaging;
using MediatR;
using Customers.Domain.Customers;
using BuildingBlocks.Contracts.Messages;

namespace Customers.Features.Events
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
            var integrationEvent = new CustomerContracts.CustomerCreated(createdEvent.Customer.Id);
            await _messageBus.Publish(integrationEvent, cancellationToken);
        }
    }
}

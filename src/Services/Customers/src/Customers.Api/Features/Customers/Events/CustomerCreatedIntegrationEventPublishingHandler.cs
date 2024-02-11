using BuildingBlocks.Messaging;
using Customers.Api.Domain.CustomerAggregate;
using Customers.Messages.Events;
using MediatR;

namespace Customers.Api.Features.Customers.Events;

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
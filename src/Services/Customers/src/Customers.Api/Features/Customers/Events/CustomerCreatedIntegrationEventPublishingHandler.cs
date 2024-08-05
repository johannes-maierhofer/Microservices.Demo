using Argo.MD.BuildingBlocks.Messaging;
using Argo.MD.Customers.Api.Domain.CustomerAggregate;
using Argo.MD.Customers.Messages.Events;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Events;

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
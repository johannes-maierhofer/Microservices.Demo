using Argo.MD.BuildingBlocks.Messaging;
using Argo.MD.Customers.Api.Domain.CustomerAggregate.Events;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Events;

public class CustomerCreatedMessagePublishingHandler(IMessageBus messageBus)
    : INotificationHandler<CustomerCreated>
{
    public async Task Handle(CustomerCreated @event, CancellationToken cancellationToken)
    {
        var integrationEvent = new Messages.CustomerCreated(
            @event.Customer.Id,
            @event.Customer.FirstName,
            @event.Customer.LastName,
            @event.Customer.EmailAddress);

        await messageBus.Publish(integrationEvent, cancellationToken);
    }
}
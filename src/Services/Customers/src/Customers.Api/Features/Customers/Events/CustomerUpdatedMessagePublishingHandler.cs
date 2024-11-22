using Argo.MD.BuildingBlocks.Messaging;
using Argo.MD.Customers.Api.Domain.CustomerAggregate.Events;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Events;

public class CustomerUpdatedMessagePublishingHandler(IMessageBus messageBus)
    : INotificationHandler<CustomerUpdated>
{
    public async Task Handle(CustomerUpdated @event, CancellationToken cancellationToken)
    {
        var integrationEvent = new Messages.CustomerUpdated(
            @event.Customer.Id,
            @event.Customer.FirstName,
            @event.Customer.LastName,
            @event.Customer.EmailAddress);

        await messageBus.Publish(integrationEvent, cancellationToken);
    }
}
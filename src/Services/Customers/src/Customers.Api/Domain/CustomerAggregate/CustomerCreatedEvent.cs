using Argo.MD.BuildingBlocks.Core.Domain.Events;

namespace Argo.MD.Customers.Api.Domain.CustomerAggregate;

public class CustomerCreatedEvent : DomainEvent
{
    public CustomerCreatedEvent(Customer customer)
    {
        Customer = customer;
    }

    public Customer Customer { get; }
}
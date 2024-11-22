using Argo.MD.BuildingBlocks.Core.Domain.Events;

namespace Argo.MD.Customers.Api.Domain.CustomerAggregate.Events;

public record CustomerCreated(Customer Customer) : IDomainEvent;
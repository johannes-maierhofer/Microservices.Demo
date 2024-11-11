namespace Argo.MD.Customers.Messages.Events;

public record NonIdempotentCustomerCreated(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
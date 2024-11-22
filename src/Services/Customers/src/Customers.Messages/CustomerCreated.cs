namespace Argo.MD.Customers.Messages;

public record CustomerCreated(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
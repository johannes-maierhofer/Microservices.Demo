namespace Argo.MD.Customers.Messages;

public record CustomerUpdated(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
namespace Customers.Api.Contracts.Customers;

public record CustomerDetailsResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
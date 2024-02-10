namespace Customers.Features.Customers.Queries.GetCustomerDetails;

public record CustomerDetailsDto(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
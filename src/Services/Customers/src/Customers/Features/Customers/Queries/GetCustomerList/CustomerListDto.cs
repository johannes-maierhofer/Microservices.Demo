namespace Customers.Features.Customers.Queries.GetCustomerList;

public record CustomerListDto(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
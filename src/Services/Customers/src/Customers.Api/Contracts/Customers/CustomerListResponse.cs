namespace Customers.Api.Contracts.Customers;

public record CustomerListResponse(
    List<CustomerListItem> Items,
    int PageNumber,
    int TotalPages,
    int TotalCount);

public record CustomerListItem(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);
namespace Argo.MD.Customers.Api.Contracts.Customers;

public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string EmailAddress
);
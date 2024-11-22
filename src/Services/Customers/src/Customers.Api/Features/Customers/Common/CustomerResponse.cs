namespace Argo.MD.Customers.Api.Features.Customers.Common;

public record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress);

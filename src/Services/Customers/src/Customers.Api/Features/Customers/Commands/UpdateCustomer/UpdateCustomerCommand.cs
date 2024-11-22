using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.Customers.Api.Features.Customers.Common;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress)
    : ICommand<CustomerResponse>;
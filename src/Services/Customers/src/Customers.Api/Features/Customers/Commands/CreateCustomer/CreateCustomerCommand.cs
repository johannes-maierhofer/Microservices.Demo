using Argo.MD.BuildingBlocks.Mediatr;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string EmailAddress)
    : ICommand<Guid>;
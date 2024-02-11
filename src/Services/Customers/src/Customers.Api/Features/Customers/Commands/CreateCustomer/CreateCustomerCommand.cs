using BuildingBlocks.Mediatr;

namespace Customers.Api.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string EmailAddress)
    : ICommand<Guid>;
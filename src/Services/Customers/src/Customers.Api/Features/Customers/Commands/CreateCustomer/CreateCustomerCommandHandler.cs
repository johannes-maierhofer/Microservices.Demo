using Argo.MD.Customers.Api.Features.Customers.Common;
using Argo.MD.Customers.Api.Persistence;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(CustomerDbContext dbContext)
    : IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
    {
        var customer = Domain.CustomerAggregate.Customer.Create(
            cmd.FirstName,
            cmd.LastName,
            cmd.EmailAddress);

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerResponse(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.EmailAddress);
    }
}
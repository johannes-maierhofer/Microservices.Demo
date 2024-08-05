using Argo.MD.Customers.Api.Persistence;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly CustomerDbContext _dbContext;

    public CreateCustomerCommandHandler(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
    {
        var customer = Domain.CustomerAggregate.Customer.Create(
            cmd.FirstName,
            cmd.LastName,
            cmd.EmailAddress);

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
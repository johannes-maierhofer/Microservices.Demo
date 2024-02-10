using Customers.Domain.CustomerAggregate;
using Customers.Persistence;
using MediatR;

namespace Customers.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly CustomerDbContext _dbContext;

    public CreateCustomerCommandHandler(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            cmd.FirstName,
            cmd.LastName,
            cmd.EmailAddress);

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
using BuildingBlocks.Mediatr;
using Customers.Domain.Customers;
using Customers.Persistence;
using MediatR;

namespace Customers.Features.Commands.CreateCustomer
{
    public record CreateCustomerCommand(
            string FirstName,
            string LastName,
            string EmailAddress)
        : ICommand<Guid>;

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
}

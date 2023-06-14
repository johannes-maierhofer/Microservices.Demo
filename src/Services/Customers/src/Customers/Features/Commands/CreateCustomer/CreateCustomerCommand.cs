using BuildingBlocks.Contracts.Messages;
using BuildingBlocks.Mediatr;
using BuildingBlocks.Messaging;
using Customers.Domain;
using Customers.Persistence;
using MediatR;

namespace Customers.Features.Commands.CreateCustomer
{
    public record CreateCustomerCommand : ICommand<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly CustomerDbContext _dbContext;
        private readonly IMessageBus _messageBus;

        public CreateCustomerCommandHandler(
            CustomerDbContext dbContext,
            IMessageBus messageBus)
        {
            _dbContext = dbContext;
            _messageBus = messageBus;
        }

        public async Task<Guid> Handle(CreateCustomerCommand cmd, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                FirstName = cmd.FirstName,
                LastName = cmd.LastName,
                EmailAddress = cmd.EmailAddress
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _messageBus.Publish(new CustomerContracts.CustomerCreated(customer.Id), cancellationToken);

            return customer.Id;
        }
    }
}

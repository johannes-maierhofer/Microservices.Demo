using BuildingBlocks.Exceptions;
using BuildingBlocks.Mediatr;
using Customers.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customers.Features.Queries.GetCustomer
{
    public record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerDto>;

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly CustomerDbContext _dbContext;

        public GetCustomerQueryHandler(CustomerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CustomerDto> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
        {
            var customerDto = await _dbContext
                .Customers
                .Where(c => c.Id == query.CustomerId)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    EmailAddress = c.EmailAddress
                })
                .FirstOrDefaultAsync(cancellationToken);

            return customerDto ?? throw new NotFoundException("A customer with the given id was not found.");
        }
    }
}

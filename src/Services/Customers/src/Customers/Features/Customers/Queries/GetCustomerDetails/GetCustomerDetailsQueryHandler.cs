using BuildingBlocks.Core.Exceptions;
using Customers.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customers.Features.Customers.Queries.GetCustomerDetails;

public class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, CustomerDetailsDto>
{
    private readonly CustomerDbContext _dbContext;

    public GetCustomerDetailsQueryHandler(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerDetailsDto> Handle(GetCustomerDetailsQuery query, CancellationToken cancellationToken)
    {
        var customerDetailsDto = await _dbContext
            .Customers
            .Where(c => c.Id == query.CustomerId)
            .Select(c => new CustomerDetailsDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.EmailAddress))
            .FirstOrDefaultAsync(cancellationToken);

        return customerDetailsDto ?? throw new NotFoundException("A customer with the given id was not found.");
    }
}
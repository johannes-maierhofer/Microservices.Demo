using BuildingBlocks.Core.Exceptions;
using Customers.Api.Contracts.Customers;
using Customers.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Customers.Api.Features.Customers.Queries.GetCustomerDetails;

public class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, CustomerDetailsResponse>
{
    private readonly CustomerDbContext _dbContext;

    public GetCustomerDetailsQueryHandler(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerDetailsResponse> Handle(GetCustomerDetailsQuery query, CancellationToken cancellationToken)
    {
        var customerDetailsDto = await _dbContext
            .Customers
            .Where(c => c.Id == query.CustomerId)
            .Select(c => new CustomerDetailsResponse(
                c.Id,
                c.FirstName,
                c.LastName,
                c.EmailAddress))
            .FirstOrDefaultAsync(cancellationToken);

        return customerDetailsDto ?? throw new NotFoundException("A customer with the given id was not found.");
    }
}
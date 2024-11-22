using Argo.MD.BuildingBlocks.Core.Exceptions;
using Argo.MD.Customers.Api.Features.Customers.Common;
using Argo.MD.Customers.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomer;

public class GetCustomerQueryHandler(CustomerDbContext dbContext) : IRequestHandler<GetCustomerQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        var result = await dbContext
            .Customers
            .Where(c => c.Id == query.CustomerId)
            .Select(c => new CustomerResponse(
                c.Id,
                c.FirstName,
                c.LastName,
                c.EmailAddress))
            .FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new NotFoundException($"Customer with id '{query.CustomerId}' not found.");
    }
}

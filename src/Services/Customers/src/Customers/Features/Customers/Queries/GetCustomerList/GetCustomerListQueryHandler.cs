using BuildingBlocks.Core.Mappings;
using BuildingBlocks.Core.Models;
using Customers.Persistence;
using MediatR;

namespace Customers.Features.Customers.Queries.GetCustomerList;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, PaginatedList<CustomerListDto>>
{
    private readonly CustomerDbContext _dbContext;

    public GetCustomerListQueryHandler(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<CustomerListDto>> Handle(GetCustomerListQuery query, CancellationToken cancellationToken)
    {
        var customerList = await _dbContext
            .Customers
            .Select(c => new CustomerListDto(
                c.Id,
                c.FirstName,
                c.LastName,
                c.EmailAddress))
            .ToPaginatedListAsync(
                query.PageNumber,
                query.PageSize,
                cancellationToken);

        return customerList;
    }
}
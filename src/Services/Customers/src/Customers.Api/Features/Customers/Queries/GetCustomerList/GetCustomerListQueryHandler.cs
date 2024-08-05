using Argo.MD.BuildingBlocks.Core.Mappings;
using Argo.MD.Customers.Api.Contracts.Customers;
using Argo.MD.Customers.Api.Persistence;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomerList;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, CustomerListResponse>
{
    private readonly CustomerDbContext _dbContext;

    public GetCustomerListQueryHandler(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CustomerListResponse> Handle(GetCustomerListQuery query, CancellationToken cancellationToken)
    {
        var result = await _dbContext
            .Customers
            .Select(c => new CustomerListItem(
                c.Id,
                c.FirstName,
                c.LastName,
                c.EmailAddress))
            .ToPaginatedListAsync(
                query.PageNumber,
                query.PageSize,
                cancellationToken);

        return new CustomerListResponse(
            result.Items.ToList(),
            result.PageNumber,
            result.TotalPages,
            result.TotalCount);
    }
}
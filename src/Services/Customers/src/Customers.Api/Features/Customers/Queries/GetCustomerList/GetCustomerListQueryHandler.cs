using Argo.MD.BuildingBlocks.Core.Mappings;
using Argo.MD.Customers.Api.Features.Customers.Common;
using Argo.MD.Customers.Api.Persistence;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomerList;

public class GetCustomerListQueryHandler(CustomerDbContext dbContext)
    : IRequestHandler<GetCustomerListQuery, CustomerListResponse>
{
    public async Task<CustomerListResponse> Handle(GetCustomerListQuery query, CancellationToken cancellationToken)
    {
        var result = await dbContext
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

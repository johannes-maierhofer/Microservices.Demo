using BuildingBlocks.Core.Models;
using BuildingBlocks.Mediatr;

namespace Customers.Features.Customers.Queries.GetCustomerList;

public record GetCustomerListQuery(
    int PageNumber = 1,
    int PageSize  = 10
) : IQuery<PaginatedList<CustomerListDto>>;
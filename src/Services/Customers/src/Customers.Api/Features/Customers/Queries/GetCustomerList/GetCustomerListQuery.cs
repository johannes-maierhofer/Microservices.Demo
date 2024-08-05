using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.Customers.Api.Contracts.Customers;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomerList;

public record GetCustomerListQuery(
    int PageNumber = 1,
    int PageSize  = 10
) : IQuery<CustomerListResponse>;
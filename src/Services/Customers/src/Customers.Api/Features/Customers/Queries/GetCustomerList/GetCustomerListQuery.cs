using BuildingBlocks.Mediatr;
using Customers.Api.Contracts.Customers;

namespace Customers.Api.Features.Customers.Queries.GetCustomerList;

public record GetCustomerListQuery(
    int PageNumber = 1,
    int PageSize  = 10
) : IQuery<CustomerListResponse>;
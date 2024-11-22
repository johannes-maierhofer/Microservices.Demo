using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.Customers.Api.Features.Customers.Common;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomer;

public record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerResponse>;

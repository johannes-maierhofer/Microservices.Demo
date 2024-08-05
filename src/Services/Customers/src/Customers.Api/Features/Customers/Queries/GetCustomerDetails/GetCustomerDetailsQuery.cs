using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.Customers.Api.Contracts.Customers;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomerDetails;

public record GetCustomerDetailsQuery(Guid CustomerId) : IQuery<CustomerDetailsResponse>;
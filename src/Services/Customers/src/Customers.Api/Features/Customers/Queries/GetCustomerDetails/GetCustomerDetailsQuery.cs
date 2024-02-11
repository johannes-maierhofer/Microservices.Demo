using BuildingBlocks.Mediatr;
using Customers.Api.Contracts.Customers;

namespace Customers.Api.Features.Customers.Queries.GetCustomerDetails;

public record GetCustomerDetailsQuery(Guid CustomerId) : IQuery<CustomerDetailsResponse>;
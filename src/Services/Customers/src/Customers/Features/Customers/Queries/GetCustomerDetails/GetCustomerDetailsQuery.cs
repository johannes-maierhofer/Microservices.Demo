using BuildingBlocks.Mediatr;

namespace Customers.Features.Customers.Queries.GetCustomerDetails;

public record GetCustomerDetailsQuery(Guid CustomerId) : IQuery<CustomerDetailsDto>;
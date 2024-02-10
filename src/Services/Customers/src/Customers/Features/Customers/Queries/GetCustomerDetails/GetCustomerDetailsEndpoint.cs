using BuildingBlocks.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Customers.Features.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsEndpoint : IMinimalEndpoint
    {
        public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapGet($"{EndpointConfig.BaseApiPath}/customers/{{id}}", async (
                    Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetCustomerDetailsQuery(id), cancellationToken);
                    return Results.Ok(result);
                })
                // .RequireAuthorization()
                .WithName("GetCustomerDetails")
                .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
                .Produces<CustomerDetailsDto>()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get customer details")
                .WithDescription("Get customer details")
                .WithOpenApi()
                .HasApiVersion(1.0);

            return builder;
        }
    }
}

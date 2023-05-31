using BuildingBlocks.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Customers.Features.Queries.GetCustomer
{
    public class GetCustomerEndpoint : IMinimalEndpoint
    {
        public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
        {
            builder.MapGet($"{EndpointConfig.BaseApiPath}/customers/{{id}}", async (Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(new GetCustomerQuery(id), cancellationToken);
                    return Results.Ok(result);
                })
                // .RequireAuthorization()
                .WithName("GetCustomer")
                .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
                .Produces<CustomerDto>()
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get customer")
                .WithDescription("Get customer")
                .WithOpenApi()
                .HasApiVersion(1.0);

            return builder;
        }
    }
}

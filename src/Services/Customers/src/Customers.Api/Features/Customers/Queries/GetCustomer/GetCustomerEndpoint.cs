using Argo.MD.BuildingBlocks.Web;
using Argo.MD.Customers.Api.Features.Customers.Common;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Queries.GetCustomer;

public class GetCustomerEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/customers/{{id}}", async (
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCustomerQuery(id), cancellationToken);
                return Results.Ok(result);
            })
            // .RequireAuthorization()
            .WithName("GetCustomer")
            .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
            .Produces<CustomerResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get customer")
            .WithDescription("Get customer")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
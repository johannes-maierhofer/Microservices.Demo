using BuildingBlocks.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Customers.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/customers", async (
                CreateCustomerCommand command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            // .RequireAuthorization()
            .WithName("CreateCustomer")
            .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
            .Produces<Guid>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create customer")
            .WithDescription("Create customer")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
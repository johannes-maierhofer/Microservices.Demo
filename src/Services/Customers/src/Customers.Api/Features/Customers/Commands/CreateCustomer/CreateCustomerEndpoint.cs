using BuildingBlocks.Web;
using Customers.Api.Contracts.Customers;
using Mapster;
using MediatR;

namespace Customers.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost($"{EndpointConfig.BaseApiPath}/customers", async (
                CreateCustomerRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Adapt<CreateCustomerCommand>();
                var result = await mediator.Send(command, cancellationToken);

                return Results.Ok(new CreateCustomerResponse(result));
            })
            // .RequireAuthorization()
            .WithName("CreateCustomer")
            .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
            .Produces<CreateCustomerResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create customer")
            .WithDescription("Create customer")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
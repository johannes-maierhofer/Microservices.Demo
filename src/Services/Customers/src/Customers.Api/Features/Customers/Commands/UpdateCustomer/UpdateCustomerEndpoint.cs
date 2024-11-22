using Argo.MD.BuildingBlocks.Web;
using Argo.MD.Customers.Api.Features.Customers.Common;
using Mapster;
using MediatR;

namespace Argo.MD.Customers.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut($"{EndpointConfig.BaseApiPath}/customers", async (
                UpdateCustomerRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Adapt<UpdateCustomerCommand>();
                var result = await mediator.Send(command, cancellationToken);

                return Results.Ok(result);
            })
            // .RequireAuthorization()
            .WithName("UpdateCustomer")
            .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
            .Produces<CustomerResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update customer")
            .WithDescription("Update customer")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}

public record UpdateCustomerRequest(
    Guid Id,
    string FirstName,
    string LastName,
    string EmailAddress
);

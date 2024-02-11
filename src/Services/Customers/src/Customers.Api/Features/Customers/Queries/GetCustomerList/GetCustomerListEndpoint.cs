using BuildingBlocks.Web;
using Customers.Api.Contracts.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Features.Customers.Queries.GetCustomerList;

public class GetCustomerListEndpoint : IMinimalEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet($"{EndpointConfig.BaseApiPath}/customers", async (
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetCustomerListQuery(pageNumber, pageSize),
                    cancellationToken);

                return Results.Ok(result);
            })
            // .RequireAuthorization()
            .WithName("GetCustomerList")
            .WithApiVersionSet(builder.NewApiVersionSet("Customers").Build())
            .Produces<CustomerListResponse>()
            .WithSummary("Get customer List")
            .WithDescription("Get customer List")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
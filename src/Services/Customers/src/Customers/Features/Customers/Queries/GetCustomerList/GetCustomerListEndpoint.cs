using BuildingBlocks.Core.Models;
using BuildingBlocks.Web;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Customers.Features.Customers.Queries.GetCustomerList;

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
            .Produces<PaginatedList<CustomerListDto>>()
            .WithSummary("Get customer List")
            .WithDescription("Get customer List")
            .WithOpenApi()
            .HasApiVersion(1.0);

        return builder;
    }
}
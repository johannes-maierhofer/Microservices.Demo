using Microsoft.AspNetCore.Routing;

namespace Argo.MD.BuildingBlocks.Web;

public interface IMinimalEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}

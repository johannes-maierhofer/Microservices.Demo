using Argo.MD.BuildingBlocks.Tracing;
using MediatR;

namespace Argo.MD.BuildingBlocks.Mediatr.Behaviors
{
    public class TracingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            using (Telemetry.ActivitySource?.StartActivity(requestName + " send"))
            {
                var result = await next();
                return result;
            }
        }
    }
}

using Dapr.Client;

namespace Argo.MD.BuildingBlocks.Messaging.Dapr;

public class DaprMessageBus(DaprClient daprClient) : IMessageBus
{
    public Task Send(object message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task Publish(object message, CancellationToken cancellationToken = default)
    {
        await daprClient.PublishEventAsync(
            DaprConstants.DaprPubSubName,
            message.GetType().Name,
            message,
            cancellationToken);
    }
}
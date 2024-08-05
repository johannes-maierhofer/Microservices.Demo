using MassTransit;

namespace Argo.MD.BuildingBlocks.Messaging.MassTransit
{
    public class MassTransitMessageBus : IMessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private const int TimeoutInMilliseconds = 20000;

        public MassTransitMessageBus(
            IPublishEndpoint publishEndpoint,
            ISendEndpointProvider sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Send(object message, CancellationToken cancellationToken = default)
        {
            var innerCancellationTokenSource = new CancellationTokenSource(TimeoutInMilliseconds);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(innerCancellationTokenSource.Token, cancellationToken);

            await _sendEndpointProvider.Send(message, linkedTokenSource.Token);
        }

        public async Task Publish(object message, CancellationToken cancellationToken = default)
        {
            var innerCancellationTokenSource = new CancellationTokenSource(TimeoutInMilliseconds);
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(innerCancellationTokenSource.Token, cancellationToken);

            await _publishEndpoint.Publish(message, linkedTokenSource.Token);
        }
    }
}

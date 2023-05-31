using MassTransit;

namespace BuildingBlocks.Messaging.MassTransit
{
    public class MassTransitMessageBus : IMessageBus
    {
        private readonly IBus _bus;

        public MassTransitMessageBus(IBus bus)
        {
            _bus = bus;
        }

        public async Task Send(object message, CancellationToken cancellationToken = default)
        {
            await _bus.Send(message, cancellationToken);
        }

        public async Task Publish(object message, CancellationToken cancellationToken = default)
        {
            await _bus.Publish(message, cancellationToken);
        }
    }
}

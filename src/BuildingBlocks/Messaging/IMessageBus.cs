namespace BuildingBlocks.Messaging
{
    public interface IMessageBus
    {
        // TODO: probably better to get rid of the Send method in IMessageBus
        Task Send(object message, CancellationToken cancellationToken = default);
        Task Publish(object message, CancellationToken cancellationToken = default);
    }
}

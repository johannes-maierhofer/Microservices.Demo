namespace BuildingBlocks.Messaging
{
    public interface IMessageBus
    {
        Task Send(object message, CancellationToken cancellationToken = default);
        Task Publish(object message, CancellationToken cancellationToken = default);
    }
}

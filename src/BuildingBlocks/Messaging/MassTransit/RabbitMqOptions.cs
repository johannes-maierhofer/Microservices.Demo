namespace Argo.MD.BuildingBlocks.Messaging.MassTransit;

public class RabbitMqOptions
{
    public string HostName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public ushort? Port { get; init; }
}

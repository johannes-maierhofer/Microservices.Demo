using MassTransit;

namespace Argo.MD.BuildingBlocks.Messaging.MassTransit;

public class CustomEntityNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"custom-prefix-{typeof(T).Name}";
    }
}

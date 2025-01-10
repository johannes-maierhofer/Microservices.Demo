using Microsoft.Extensions.DependencyInjection;

namespace Argo.MD.BuildingBlocks.Messaging.Dapr;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddDaprMessageBus(this IServiceCollection services)
    {
        services.AddScoped<IMessageBus, DaprMessageBus>();

        return services;
    }
}

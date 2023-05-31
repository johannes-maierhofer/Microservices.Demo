using System.Reflection;
using BuildingBlocks.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Messaging.MassTransit;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services,
        WebApplicationBuilder builder,
        Assembly assembly)
    {
        services.AddScoped<IMessageBus, MassTransitMessageBus>();

        services.AddValidateOptions<RabbitMqOptions>();

        if (builder.Environment.IsEnvironment("test"))
        {
            services.AddMassTransitTestHarness(configure =>
            {
                SetupMasstransitConfigurations(
                    services,
                    builder,
                    configure,
                    assembly);
            });
            return services;
        }

        services.AddMassTransit(configure =>
        {
            SetupMasstransitConfigurations(
                services,
                builder,
                configure,
                assembly);
        });

        return services;
    }

    private static void SetupMasstransitConfigurations(
        IServiceCollection services,
        WebApplicationBuilder builder,
        IBusRegistrationConfigurator configure,
        Assembly assembly)
    {
        var appOptions = builder.Configuration
            .GetSection("App")
            .Get<AppOptions>();

        configure.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(appOptions!.Name + "-", false));
        configure.AddConsumers(assembly);

        configure.UsingRabbitMq((ctx, configurator) =>
        {
            var rabbitMqOptions = services.GetOptions<RabbitMqOptions>("RabbitMq");

            configurator.Host(rabbitMqOptions.HostName, rabbitMqOptions.Port ?? 5672, "/", h =>
            {
                h.Username(rabbitMqOptions.UserName);
                h.Password(rabbitMqOptions.Password);
            });

            configurator.ConfigureEndpoints(ctx);
        });
    }
}

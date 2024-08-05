using System.Reflection;
using Argo.MD.BuildingBlocks.Configuration;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Argo.MD.BuildingBlocks.Messaging.MassTransit;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Add MassTransit with a DbContext to be used as an outbox.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomMassTransit<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env,
        Assembly assembly)
        where TDbContext : DbContext
    {
        services.AddScoped<IMessageBus, MassTransitMessageBus>();
        services.AddValidateOptions<RabbitMqOptions>();

        if (env.IsEnvironment("test"))
        {
            services.AddMassTransitTestHarness(configure =>
            {
                SetupMasstransitConfigurations(
                    services,
                    configuration,
                    configure,
                    assembly);

                configure.AddEntityFrameworkOutbox<TDbContext>(o =>
                {
                    o.UseBusOutbox();
                    o.UseSqlServer();
                });
            });
            return services;
        }

        services.AddMassTransit(configure =>
        {
            SetupMasstransitConfigurations(
                services,
                configuration,
                configure,
                assembly);

            configure.AddEntityFrameworkOutbox<TDbContext>(o =>
            {
                o.QueryDelay = TimeSpan.FromSeconds(1);

                o.UseSqlServer();
                o.UseBusOutbox();
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env,
        Assembly assembly)
    {
        services.AddScoped<IMessageBus, MassTransitMessageBus>();
        services.AddValidateOptions<RabbitMqOptions>();

        if (env.IsEnvironment("test"))
        {
            services.AddMassTransitTestHarness(configure =>
            {
                SetupMasstransitConfigurations(
                    services,
                    configuration,
                    configure,
                    assembly);
            });
            return services;
        }

        services.AddMassTransit(configure =>
        {
            SetupMasstransitConfigurations(
                services,
                configuration,
                configure,
                assembly);
        });

        return services;
    }

    private static void SetupMasstransitConfigurations(
        IServiceCollection services,
        IConfiguration configuration,
        IBusRegistrationConfigurator configure,
        Assembly assembly)
    {
        var appOptions = configuration
            .GetSection("App")
            .Get<AppOptions>();

        configure.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(appOptions!.Name + "--", false));
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

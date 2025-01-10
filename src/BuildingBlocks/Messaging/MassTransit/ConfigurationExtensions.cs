using System.Reflection;
using Argo.MD.BuildingBlocks.Configuration;
using CloudEventify;
using CloudEventify.MassTransit;
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
    /// <param name="consumersFromAssembly"></param>
    /// <param name="messagesAssemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomMassTransit<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env,
        Assembly consumersFromAssembly,
        IEnumerable<Assembly> messagesAssemblies)
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
                    consumersFromAssembly,
                    messagesAssemblies);

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
                consumersFromAssembly,
                messagesAssemblies);

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
        Assembly assembly,
        IEnumerable<Assembly> messagesAssemblies)
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
                    assembly,
                    messagesAssemblies);
            });
            return services;
        }

        services.AddMassTransit(configure =>
        {
            SetupMasstransitConfigurations(
                services,
                configuration,
                configure,
                assembly,
                messagesAssemblies);
        });

        return services;
    }

    private static void SetupMasstransitConfigurations(
        IServiceCollection services,
        IConfiguration configuration,
        IBusRegistrationConfigurator configure,
        Assembly consumersFromAssembly,
        IEnumerable<Assembly> messagesAssemblies)
    {
        var appOptions = configuration
            .GetSection("App")
            .Get<AppOptions>();

        // configure.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(appOptions!.Name + "--", false));
        configure.AddConsumers(consumersFromAssembly);
        
        configure.UsingRabbitMq((ctx, rmqConfig) =>
        {
            var rabbitMqOptions = services.GetOptions<RabbitMqOptions>("RabbitMq");

            rmqConfig.Host(rabbitMqOptions.HostName, rabbitMqOptions.Port ?? 5672, "/", h =>
            {
                h.Username(rabbitMqOptions.UserName);
                h.Password(rabbitMqOptions.Password);
            });

            rmqConfig
                .UseCloudEvents()
                .WithTypes(m => m.MapAllTypes(messagesAssemblies.ToArray()));

            rmqConfig.ConfigureEndpoints(ctx);
        });
    }

    private static IMap MapAllTypes(this IMap typeMapper, params Assembly[] assemblies)
    {
        var mapMethod = typeof(IMap)
            .GetMethods()
            .FirstOrDefault(m => m.Name == "Map" && m.IsGenericMethod);

        if (mapMethod == null)
        {
            throw new InvalidOperationException("Map method not found on CloudEventify.IMap.");
        }

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass &&
                            !t.IsAbstract &&
                            !typeof(Attribute).IsAssignableFrom(t))
                .ToList();

            foreach (var type in types)
            {
                try
                {
                    var genericMapMethod = mapMethod.MakeGenericMethod(type);
                    genericMapMethod.Invoke(typeMapper, new object[] { type.FullName! });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to map type {type.FullName}: {ex.Message}");
                }
            }
        }

        return typeMapper;
    }
}

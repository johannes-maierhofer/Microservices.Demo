using BuildingBlocks.Configuration;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.SqlServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BuildingBlocks.HealthCheck;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services)
    {
        var healthOptions = services.GetOptions<HealthOptions>("Health");

        if (!healthOptions.Enabled) 
            return services;

        var appOptions = services.GetOptions<AppOptions>("App");
        
        var healthChecksBuilder = services.AddHealthChecks();

        var rabbitMqOptions = services.GetOptions<RabbitMqOptions>("RabbitMq");
        if (!string.IsNullOrEmpty(rabbitMqOptions.HostName))
        {
            healthChecksBuilder
                .AddRabbitMQ(
                    rabbitConnectionString:
                    $"amqp://{rabbitMqOptions.UserName}:{rabbitMqOptions.Password}@{rabbitMqOptions.HostName}");
        }

        var sqlServerOptions = services.GetOptions<SqlServerOptions>("SqlServer");
        if (!string.IsNullOrEmpty(sqlServerOptions.ConnectionString))
        {
            healthChecksBuilder.AddSqlServer(sqlServerOptions.ConnectionString);
        }

        services.AddHealthChecksUI(setup =>
            {
                setup.SetEvaluationTimeInSeconds(60); // time in seconds between check
                setup.AddHealthCheckEndpoint($"Basic Health Check - {appOptions.Name}", "/healthz");
            })
            .AddSqlServerStorage(healthOptions.SqlServerStorageConnectionString);

        return services;
    }

    public static WebApplication UseCustomHealthCheck(this WebApplication app)
    {
        var healthOptions = app.Configuration.GetOptions<HealthOptions>(nameof(HealthOptions));

        if (!healthOptions.Enabled) return app;

        app.UseHealthChecks("/healthz",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                })
            .UseHealthChecksUI(options =>
            {
                options.ApiPath = "/healthcheck";
                options.UIPath = "/healthcheck-ui";
            });

        return app;
    }
}

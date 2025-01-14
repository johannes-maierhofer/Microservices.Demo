﻿using Argo.MD.BuildingBlocks.Configuration;
using Argo.MD.BuildingBlocks.Messaging.MassTransit;
using Argo.MD.BuildingBlocks.SqlServer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Argo.MD.BuildingBlocks.HealthCheck;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services)
    {
        var healthOptions = services.GetOptions<HealthOptions>("Health");

        if (!healthOptions.Enabled) 
            return services;

        var healthChecksBuilder = services.AddHealthChecks();

        //var rabbitMqOptions = services.GetOptions<RabbitMqOptions>("RabbitMq");
        //if (!string.IsNullOrEmpty(rabbitMqOptions.HostName))
        //{
        //    healthChecksBuilder
        //        .AddRabbitMQ(
        //            rabbitConnectionString:
        //            $"amqp://{rabbitMqOptions.UserName}:{rabbitMqOptions.Password}@{rabbitMqOptions.HostName}");
        //}

        var sqlServerOptions = services.GetOptions<SqlServerOptions>("SqlServer");
        if (!string.IsNullOrEmpty(sqlServerOptions.ConnectionString))
        {
            healthChecksBuilder.AddSqlServer(sqlServerOptions.ConnectionString);
        }

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
                });

        return app;
    }
}

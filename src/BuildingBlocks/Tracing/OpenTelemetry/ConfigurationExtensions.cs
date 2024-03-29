using System.Diagnostics;
using BuildingBlocks.Configuration;
using BuildingBlocks.Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BuildingBlocks.Tracing.OpenTelemetry;

/*
 * ref. https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/326
 * MassTransit instrumentation is not needed in MassTransit 8, simply use AddSource("MassTransit"), see below
 */

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
    {
        var appOptions = services.GetOptions<AppOptions>("App");
        var logOptions = services.GetOptions<LogOptions>("Log");

        // add custom ActivitySource for application
        Telemetry.ActivitySource = new ActivitySource(appOptions.Name);

        // configure tracing
        services
            .AddOpenTelemetry()
            .WithTracing(builder => builder
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter = context =>
                    {
                        if (context.Request.Path.ToString().EndsWith("/healthz"))
                            return false;

                        return true;
                    };
                })
                .AddHttpClientInstrumentation(options =>
                {
                    options.FilterHttpRequestMessage = message =>
                    {
                        // ignore http calls to seq
                        var requestUrl = message.RequestUri?.ToString() ?? string.Empty;
                        if (requestUrl.StartsWith(logOptions.Seq.ServiceUrl))
                            return false;

                        // ignore http calls to health-check
                        if (requestUrl.ToLower().EndsWith("/healthz"))
                            return false;

                        return true;
                    };
                })
                .SetResourceBuilder(ResourceBuilder
                    .CreateDefault()
                    .AddService(appOptions.Name))
                .AddSource("MassTransit")
                .AddSource(appOptions.Name) // app-specific source from Telemetry.ActivitySource
                .AddJaegerExporter());

        return services;
    }
}

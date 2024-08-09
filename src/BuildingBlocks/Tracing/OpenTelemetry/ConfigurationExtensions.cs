using System.Diagnostics;
using Argo.MD.BuildingBlocks.Configuration;
using Argo.MD.BuildingBlocks.Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Argo.MD.BuildingBlocks.Tracing.OpenTelemetry;

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
        var openTelemetryOptions = services.GetOptions<OpenTelemetryOption>("OpenTelemetry");

        // add custom ActivitySource for application
        Telemetry.ActivitySource = new ActivitySource(appOptions.Name);

        // configure tracing
        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
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
                    .AddSource(appOptions.Name); // app-specific source from Telemetry.ActivitySource

                if (!string.IsNullOrEmpty(openTelemetryOptions.OtlpUrl))
                {
                    builder.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(openTelemetryOptions.OtlpUrl); // Jaeger endpoint
                    });
                }
            });

        return services;
    }
}

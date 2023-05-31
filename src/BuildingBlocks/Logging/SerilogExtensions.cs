using BuildingBlocks.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;

namespace BuildingBlocks.Logging
{
    public static class SerilogExtensions
    {
        public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, _, loggerConfiguration) =>
            {
                // var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var appOptions = context.Configuration
                    .GetSection("App")
                    .Get<AppOptions>();

                var logOptions = context.Configuration
                    .GetSection("Log")
                    .Get<LogOptions>();

                var logLevel = Enum.TryParse<LogEventLevel>(logOptions!.Level, true, out var level)
                    ? level
                    : LogEventLevel.Information;

                loggerConfiguration
                    .MinimumLevel.Is(logLevel)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    // Only show ef-core information in error level
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                    // Filter out ASP.NET Core infrastructure logs that are Information and below
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithSpan() // span information from current activity
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", appOptions!.Name)
                    .ReadFrom.Configuration(context.Configuration);

                if (logOptions.Seq is { Enabled: true })
                {
                    loggerConfiguration.WriteTo.Seq(logOptions.Seq.ServiceUrl);
                }
            });

            return builder;
        }
    }
}

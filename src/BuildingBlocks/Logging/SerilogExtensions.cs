using BuildingBlocks.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;

namespace BuildingBlocks.Logging
{
    public static class SerilogExtensions
    {
        public static IServiceCollection AddCustomSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            // var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var appOptions = configuration
                .GetSection("App")
                .Get<AppOptions>();

            var logOptions = configuration
                .GetSection("Log")
                .Get<LogOptions>();

            var logLevel = Enum.TryParse<LogEventLevel>(logOptions!.Level, true, out var level)
                ? level
                : LogEventLevel.Information;

            services.AddSerilog(loggerConfiguration =>
            {
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
                    .ReadFrom.Configuration(configuration);

                if (logOptions.Seq is { Enabled: true })
                {
                    loggerConfiguration.WriteTo.Seq(logOptions.Seq.ServiceUrl);
                }
            });

            return services;
        }
    }
}

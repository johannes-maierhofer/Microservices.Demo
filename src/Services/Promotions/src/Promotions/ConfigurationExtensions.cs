using BuildingBlocks.Configuration;
using BuildingBlocks.Logging;
using BuildingBlocks.Mediatr;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Tracing.OpenTelemetry;
using BuildingBlocks.Web;
using BuildingBlocks.Web.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Promotions.Config;
using Promotions.Services;

namespace Promotions
{
    public static class ConfigurationExtensions
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.Configure<UrlsConfig>(builder.Configuration.GetSection("Urls"));

            var urlsConfig = builder.Configuration
                .GetSection("Urls")
                .Get<UrlsConfig>()!;

            builder.Services.AddHttpClient("Customers", httpClient =>
            {
                httpClient.BaseAddress = new Uri(urlsConfig.Customers);
            });

            builder.Services.AddScoped<ICustomerApiClient, CustomerApiClient>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.AddCustomSerilog();
            // builder.Services.AddJwt();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCustomSwagger(builder.Configuration, typeof(PromotionsRoot).Assembly);
            builder.Services.AddCustomVersioning();
            builder.Services.AddCustomMediatr();
            // builder.Services.AddValidatorsFromAssembly(typeof(PromotionsRoot).Assembly);
            builder.Services.AddProblemDetails();
            //builder.Services.AddCustomMapster(typeof(PromotionsRoot).Assembly);
            //builder.Services.AddCustomHealthCheck();
            builder.Services.AddCustomMassTransit(builder, typeof(PromotionsRoot).Assembly);
            builder.Services.AddCustomOpenTelemetry();
            // builder.Services.AddTransient<AuthHeaderHandler>();

            return builder;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            var env = app.Environment;
            var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

            app.UseCustomProblemDetails();

            //app.UseSerilogRequestLogging(options =>
            //{
            //    options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
            //});
            //app.UseCorrelationId();
            //app.UseHttpMetrics();
            //app.UseCustomHealthCheck();
            //app.MapMetrics();
            app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

            if (env.IsDevelopment())
            {
                app.UseCustomSwagger();
            }

            return app;
        }
    }
}

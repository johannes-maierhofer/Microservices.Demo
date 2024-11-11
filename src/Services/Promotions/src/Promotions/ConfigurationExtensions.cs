using Argo.MD.BuildingBlocks.Configuration;
using Argo.MD.BuildingBlocks.HealthCheck;
using Argo.MD.BuildingBlocks.Logging;
using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.BuildingBlocks.Messaging.MassTransit;
using Argo.MD.BuildingBlocks.Tracing.OpenTelemetry;
using Argo.MD.BuildingBlocks.Web;
using Argo.MD.BuildingBlocks.Web.Swagger;
using Argo.MD.Customers.Api.Client;
using Argo.MD.Promotions.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Argo.MD.Promotions
{
    public static class ConfigurationExtensions
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.Configure<ServiceUrlSettings>(builder.Configuration.GetSection("ServiceUrl"));

            var urlSettings = builder.Configuration
                .GetSection("ServiceUrl")
                .Get<ServiceUrlSettings>()!;

            builder.Services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(urlSettings.CustomerApi);
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // builder.Services.AddJwt();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCustomSwagger(builder.Configuration, typeof(PromotionsRoot).Assembly);
            builder.Services.AddCustomVersioning();
            builder.Services.AddCustomMediatr();
            // builder.Services.AddValidatorsFromAssembly(typeof(PromotionsRoot).Assembly);
            builder.Services.AddProblemDetails();
            //builder.Services.AddCustomMapster(typeof(PromotionsRoot).Assembly);
            builder.Services.AddCustomSerilog(builder.Configuration);
            builder.Services.AddCustomMassTransit(builder.Configuration, builder.Environment, typeof(PromotionsRoot).Assembly);
            builder.Services.AddCustomOpenTelemetry();
            // builder.Services.AddTransient<AuthHeaderHandler>();

            if (builder.Environment.UseHealthChecks())
            {
                builder.Services.AddCustomHealthCheck();
            }

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
            
            if (app.Environment.UseHealthChecks())
            {
                app.UseCustomHealthCheck();
            }

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

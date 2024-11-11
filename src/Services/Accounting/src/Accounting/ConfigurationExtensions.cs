using Argo.MD.Accounting.Config;
using Argo.MD.Accounting.Persistence;
using Argo.MD.BuildingBlocks.Configuration;
using Argo.MD.BuildingBlocks.Core.Domain.Events;
using Argo.MD.BuildingBlocks.EfCore;
using Argo.MD.BuildingBlocks.EfCore.Interceptors;
using Argo.MD.BuildingBlocks.HealthCheck;
using Argo.MD.BuildingBlocks.Logging;
using Argo.MD.BuildingBlocks.Mediatr;
using Argo.MD.BuildingBlocks.Messaging.MassTransit;
using Argo.MD.BuildingBlocks.SqlServer;
using Argo.MD.BuildingBlocks.Tracing.OpenTelemetry;
using Argo.MD.BuildingBlocks.Web;
using Argo.MD.BuildingBlocks.Web.Swagger;
using Argo.MD.Customers.Api.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Argo.MD.Accounting
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
            builder.Services
                .AddHttpContextAccessor()
                .AddCustomSwagger(builder.Configuration, typeof(AccountingRoot).Assembly)
                .AddCustomMediatr()
                .AddCustomVersioning()
                .AddProblemDetails()
                .AddCustomSerilog(builder.Configuration)
                .AddCustomMassTransit(builder.Configuration, builder.Environment, typeof(AccountingRoot).Assembly)
                .AddCustomOpenTelemetry()
                .AddPersistence();

            // builder.Services.AddValidatorsFromAssembly(typeof(PromotionsRoot).Assembly);
            // builder.Services.AddCustomMapster(typeof(PromotionsRoot).Assembly);
            // builder.Services.AddTransient<AuthHeaderHandler>();

            if (builder.Environment.UseHealthChecks())
            {
                builder.Services.AddCustomHealthCheck();
            }

            return builder;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            services.AddScoped<SavingChangesDomainEventDispatchingInterceptor>(); // must be scoped

            var sqlServerOptions = services.GetOptions<SqlServerOptions>("SqlServer");
            services.AddDbContext<AccountingDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(
                        sqlServerOptions.ConnectionString,
                        b => b.MigrationsAssembly(typeof(AccountingDbContext).Assembly.FullName));

                    // add interceptors
                    options.AddInterceptors(serviceProvider
                        .GetRequiredService<SavingChangesDomainEventDispatchingInterceptor>());
                }
            );
            services.AddScoped<IDbContext>(sp => sp.GetRequiredService<AccountingDbContext>());

            return services;
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

        public static WebApplication UseDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AccountingDbContext>();

                if (context.Database.IsSqlServer()
                    && !app.Environment.IsEnvironment("test")
                    && !app.Environment.IsEnvironment("SwaggerBuild"))
                    context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<AccountingRoot>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            return app;
        }
    }
}

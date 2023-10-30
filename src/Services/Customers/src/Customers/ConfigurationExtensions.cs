using BuildingBlocks.Configuration;
using BuildingBlocks.Core.Domain.Events;
using BuildingBlocks.EfCore.Interceptors;
using BuildingBlocks.HealthCheck;
using BuildingBlocks.Logging;
using BuildingBlocks.Mediatr;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.SqlServer;
using BuildingBlocks.Tracing.OpenTelemetry;
using BuildingBlocks.Web;
using BuildingBlocks.Web.Swagger;
using Customers.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Customers
{
    public static class ConfigurationExtensions
    {
        public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
            builder.Services.AddScoped<DomainEventDispatchingInterceptor>(); // must be scoped

            var sqlServerOptions = builder.Services.GetOptions<SqlServerOptions>("SqlServer");
            builder.Services.AddDbContext<CustomerDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(
                        sqlServerOptions.ConnectionString,
                        b => b.MigrationsAssembly(typeof(CustomerDbContext).Assembly.FullName));

                    // add interceptors
                    options.AddInterceptors(serviceProvider.GetRequiredService<DomainEventDispatchingInterceptor>());
                }
            );

            // builder.Services.AddJwt();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddCustomSwagger(builder.Configuration, typeof(CustomersRoot).Assembly);
            builder.Services.AddCustomVersioning();
            builder.Services.AddCustomMediatr();
            builder.Services.AddValidatorsFromAssembly(typeof(CustomersRoot).Assembly);
            builder.Services.AddProblemDetails();
            //builder.Services.AddCustomMapster(typeof(BookingRoot).Assembly);
            builder.Services.AddCustomHealthCheck();
            builder.Services.AddCustomSerilog(builder.Configuration);
            builder.Services.AddCustomMassTransit(builder.Configuration, builder.Environment, typeof(CustomersRoot).Assembly);
            builder.Services.AddCustomOpenTelemetry();
            // builder.Services.AddTransient<AuthHeaderHandler>();

            return builder;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            var env = app.Environment;
            var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

            app.UseCustomProblemDetails();
            app.UseSerilogRequestLogging(cfg =>
            {
                cfg.GetLevel = LogHelper.GetCustomLogEventLevel;
            });
            //app.UseCorrelationId();
            //app.UseHttpMetrics();
            //app.UseMigrationPersistMessage<PersistMessageDbContext>(env);
            app.UseCustomHealthCheck();
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
                var context = services.GetRequiredService<CustomerDbContext>();

                if (context.Database.IsSqlServer() && !app.Environment.IsEnvironment("test"))
                    context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomersRoot>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            return app;
        }
    }
}

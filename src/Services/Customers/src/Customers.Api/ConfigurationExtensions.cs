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
using Argo.MD.Customers.Api.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Argo.MD.Customers.Api;

public static class ConfigurationExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        builder.Services.AddScoped<SavingChangesDomainEventDispatchingInterceptor>(); // must be scoped

        var sqlServerOptions = builder.Services.GetOptions<SqlServerOptions>("SqlServer");
        builder.Services.AddDbContext<CustomerDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(
                    sqlServerOptions.ConnectionString,
                    b => b.MigrationsAssembly(typeof(CustomerDbContext).Assembly.FullName));

                // add interceptors
                options.AddInterceptors(serviceProvider.GetRequiredService<SavingChangesDomainEventDispatchingInterceptor>());
            }
        );
        builder.Services.AddScoped<IDbContext>(sp => sp.GetRequiredService<CustomerDbContext>());

        // builder.Services.AddJwt();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCustomSwagger(builder.Configuration, typeof(CustomersApiRoot).Assembly);
        builder.Services.AddCustomVersioning();
        builder.Services.AddCustomMediatr();
        builder.Services.AddValidatorsFromAssembly(typeof(CustomersApiRoot).Assembly);
        builder.Services.AddProblemDetails();
        //builder.Services.AddCustomMapster(typeof(CustomerApiRoot).Assembly);
        builder.Services.AddCustomSerilog(builder.Configuration);
        builder.Services.AddCustomMassTransit<CustomerDbContext>(
            builder.Configuration,
            builder.Environment,
            typeof(CustomersApiRoot).Assembly);
        builder.Services.AddCustomOpenTelemetry();
        // builder.Services.AddTransient<AuthHeaderHandler>();

        if(builder.Environment.UseHealthChecks())
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
        app.UseSerilogRequestLogging(cfg =>
        {
            cfg.GetLevel = LogHelper.GetCustomLogEventLevel;
        });
        //app.UseCorrelationId();
        //app.UseHttpMetrics();
        //app.UseMigrationPersistMessage<PersistMessageDbContext>(env);

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
            var context = services.GetRequiredService<CustomerDbContext>();

            if (context.Database.IsSqlServer() 
                && !app.Environment.IsEnvironment("test")
                && !app.Environment.IsEnvironment("SwaggerBuild"))
                context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomersApiRoot>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }

        return app;
    }
}
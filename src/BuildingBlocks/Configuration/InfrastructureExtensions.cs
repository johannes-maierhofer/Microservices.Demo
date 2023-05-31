using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Configuration
{
    public static class InfrastructureExtensions
    {
        //public static WebApplication UseInfrastructure(this WebApplication app)
        //{
        //    var env = app.Environment;
        //    var appOptions = app.GetOptions<AppOptions>(nameof(AppOptions));

        //    // app.UseCustomProblemDetails();
        //    //app.UseSerilogRequestLogging(options =>
        //    //{
        //    //    options.EnrichDiagnosticContext = LogEnrichHelper.EnrichFromRequest;
        //    //});
        //    //app.UseCorrelationId();
        //    //app.UseHttpMetrics();
        //    //app.UseMigrationPersistMessage<PersistMessageDbContext>(env);
        //    //app.UseCustomHealthCheck();
        //    //app.MapMetrics();
        //    app.MapGet("/", x => x.Response.WriteAsync(appOptions.Name));

        //    //if (env.IsDevelopment())
        //    //{
        //    //    app.UseCustomSwagger();
        //    //}

        //    return app;
        //}
    }
}

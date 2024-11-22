using Microsoft.AspNetCore.Http;
using Serilog.Events;

namespace Argo.MD.BuildingBlocks.Logging;

public static class LogHelper
{
    /// <summary>
    /// A function returning the custom <see cref="Serilog.Events.LogEventLevel" /> based on the <see cref="Microsoft.AspNetCore.Http.HttpContext" />, the number of
    /// elapsed milliseconds required for handling the request, and an <see cref="System.Exception" /> if one was thrown.
    /// The default behavior returns <see cref="Serilog.Events.LogEventLevel.Error" /> when the response status code is greater than 499 or if the
    /// <see cref="System.Exception" /> is not null.
    /// </summary>
    /// <param name="ctx">the HttpContext.</param>
    /// <param name="timeout">the number of elapsed milliseconds required for handling the request.</param>
    /// <param name="ex">The exception, if it was thrown.</param>
    /// <returns>The custom <see cref="Serilog.Events.LogEventLevel" />.</returns>
    public static LogEventLevel GetCustomLogEventLevel(HttpContext ctx, double timeout, Exception? ex)
    {
        if (ex != null)
        {
            return LogEventLevel.Error;
        }

        if (IsHealthCheckEndpoint(ctx) || IsSwaggerEndpoint(ctx))
        {
            return LogEventLevel.Verbose;
        }

        if (ctx.Response.StatusCode > 499)
        {
            return LogEventLevel.Error;
        }

        return LogEventLevel.Information;
    }

    private static bool IsHealthCheckEndpoint(HttpContext ctx)
    {
        var isHealthCheck =
            ctx.Request.Path.StartsWithSegments(new PathString("/healthz"), StringComparison.OrdinalIgnoreCase);

        return isHealthCheck;
    }

    private static bool IsSwaggerEndpoint(HttpContext ctx)
    {
        var isSwagger =
            ctx.Request.Path.StartsWithSegments(new PathString("/swagger"), StringComparison.OrdinalIgnoreCase);

        return isSwagger;
    }
}

using Microsoft.Extensions.Hosting;

namespace Argo.MD.BuildingBlocks.Configuration;

/// <summary>
///     Extension methods for <see cref="IHostEnvironment" />.
/// </summary>
public static class CustomEnvironmentExtensions
{
    /// <summary>
    ///     Checks if the current host environment name is <see cref="CustomEnvironments.Test" />.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment" />.</param>
    /// <returns>True if the environment name is <see cref="CustomEnvironments.Test" />, otherwise false.</returns>
    public static bool IsTest(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(CustomEnvironments.Test);
    }

    /// <summary>
    ///     Checks if the current host environment name is <see cref="CustomEnvironments.SwaggerBuild" />.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment" />.</param>
    /// <returns>True if the environment name is <see cref="CustomEnvironments.SwaggerBuild" />, otherwise false.</returns>
    public static bool IsSwaggerBuild(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(CustomEnvironments.SwaggerBuild);
    }

    /// <summary>
    ///     Checks if the current host environment name is <see cref="CustomEnvironments.Docker" />.
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment" />.</param>
    /// <returns>True if the environment name is <see cref="CustomEnvironments.Docker" />, otherwise false.</returns>
    public static bool IsDocker(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(CustomEnvironments.Docker);
    }

    /// <summary>
    /// Checks if the current host environment name is for local development (Docker or Development).
    /// </summary>
    /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment" />.</param>
    /// <returns>True if the environment is for local development.</returns>
    public static bool IsLocalDevelopment(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsDocker()
            || hostEnvironment.IsDevelopment();
    }

    public static bool UseHealthChecks(this IHostEnvironment hostEnvironment)
    {
        return !hostEnvironment.IsSwaggerBuild();
    }
}
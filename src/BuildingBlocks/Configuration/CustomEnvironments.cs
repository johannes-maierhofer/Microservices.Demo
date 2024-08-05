namespace Argo.MD.BuildingBlocks.Configuration;

public static class CustomEnvironments
{
    /// <summary>
    ///     Specifies the Testing environment for use in e.g. integration tests.
    /// </summary>
    public static readonly string Test = "Test";

    /// <summary>
    ///     Specifies the SwaggerBuild environment running when creating the swagger file on build.
    /// </summary>
    public static readonly string SwaggerBuild = "SwaggerBuild";

    /// <summary>
    ///     Specifies the environment for running the app together with others in Docker containers.
    /// </summary>
    public static readonly string Docker = "Docker";
}
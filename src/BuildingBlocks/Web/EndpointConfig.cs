using Asp.Versioning.Builder;

namespace Argo.MD.BuildingBlocks.Web
{
    public class EndpointConfig
    {
        public const string BaseApiPath = "api/v{version:apiVersion}";
        public static ApiVersionSet VersionSet { get; private set; } = default!;
    }
}

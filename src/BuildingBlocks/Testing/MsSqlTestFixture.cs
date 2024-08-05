using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace Argo.MD.BuildingBlocks.Testing
{
    // see Enhance "Integration tests in ASP.NET Core" by leveraging Testcontainers
    // at https://github.com/dotnet/AspNetCore.Docs/pull/28531/files

    public sealed class MsSqlTestFixture : IAsyncLifetime
    {
        private const string Username = "sa";
        private const string Password = "str0ngp@ssword";
        private const ushort MsSqlPort = 1433;
        private const ushort HostPort = 51823;
        private readonly IContainer _mssqlContainer = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithName("mssql-" + Guid.NewGuid())
            .WithPortBinding(HostPort, MsSqlPort)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQLCMDUSER", Username)
            .WithEnvironment("SQLCMDPASSWORD", Password)
            .WithEnvironment("MSSQL_SA_PASSWORD", Password)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools/bin/sqlcmd", "-Q", "SELECT 1;"))
            .Build();

        public Task InitializeAsync()
        {
            return _mssqlContainer.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _mssqlContainer.DisposeAsync().AsTask();
        }

    }
}

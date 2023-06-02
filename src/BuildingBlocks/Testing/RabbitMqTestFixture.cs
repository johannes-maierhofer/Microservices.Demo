using Testcontainers.RabbitMq;
using Xunit;

namespace BuildingBlocks.Testing
{
    public sealed class RabbitMqTestFixture : IAsyncLifetime
    {
        private const string Username = "guest";
        private const string Password = "guest";
        private const ushort Port = 5672;
        private const ushort ContainerPort = 54461;
        private const ushort ApiPort = 15672;
        private const ushort ContainerApiPort = 54464;

        private readonly RabbitMqContainer _container = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithName("rabbitmq-" + Guid.NewGuid())
            .WithUsername(Username)
            .WithPassword(Password)
            .WithPortBinding(ContainerApiPort, ApiPort)
            .WithPortBinding(ContainerPort, Port)
            .Build();

        public Task InitializeAsync()
        {
            return _container.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _container.DisposeAsync().AsTask();
        }

    }
}

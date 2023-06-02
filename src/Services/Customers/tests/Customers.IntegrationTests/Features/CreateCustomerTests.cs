using BuildingBlocks.Contracts.Messages;
using BuildingBlocks.Testing;
using Customers.Api;
using Customers.Features.Commands.CreateCustomer;
using FluentAssertions;
using MassTransit.Testing;

namespace Customers.IntegrationTests.Features
{
    public class CreateCustomerTests : TestFixture<Program>, IClassFixture<MsSqlTestFixture>,
        IClassFixture<RabbitMqTestFixture>
    {
        [Fact]
        public async Task CreateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateValidCommand();

            // Act
            var response = await SendAsync(command);

            // Assert
            response.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateCustomer_ShouldPublishCustomerCreatedEvent_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateValidCommand();

            // Act
            await SendAsync(command);

            // Assert
            var harness = ServiceProvider.GetTestHarness();
            var eventWasPublished = await harness.Published.Any<CustomerContracts.CustomerCreated>();
            eventWasPublished.Should().Be(true);
        }

        private static CreateCustomerCommand CreateValidCommand()
        {
            return new CreateCustomerCommand
            {
                FirstName = "First",
                LastName = "Last",
                EmailAddress = Guid.NewGuid() + "@test.com"
            };
        }
    }
}
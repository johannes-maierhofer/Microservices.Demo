using BuildingBlocks.Contracts.Messages;
using Customers.Features.Commands.CreateCustomer;
using FluentAssertions;
using MassTransit.Testing;

namespace Customers.IntegrationTests.Features
{
    public class CreateCustomerTests : AppWriteTestBase
    {
        public CreateCustomerTests(AppTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task CreateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateValidCommand();

            // Act
            var response = await Fixture.SendAsync(command);

            // Assert
            response.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateCustomer_ShouldPublishCustomerCreatedEvent_WhenCommandIsValid()
        {
            // Arrange
            var command = CreateValidCommand();

            // Act
            await Fixture.SendAsync(command);

            // Assert
            var harness = Fixture.ServiceProvider.GetTestHarness();
            var eventWasPublished = await harness.Published.Any<CustomerContracts.CustomerCreated>();
            eventWasPublished.Should().Be(true);
        }

        private static CreateCustomerCommand CreateValidCommand()
        {
            return new CreateCustomerCommand(
                "First",
                "Last",
                Guid.NewGuid() + "@test.com");
        }
    }
}
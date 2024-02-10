using Customers.Features.Customers.Commands.CreateCustomer;
using FluentAssertions;

namespace Customers.IntegrationTests.Features.Customers;

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

    //[Fact]
    //public async Task CreateCustomer_ShouldPublishCustomerCreatedEvent_WhenCommandIsValid()
    //{
    //    // Arrange
    //    var command = CreateValidCommand();

    //    // Act
    //    await Fixture.SendAsync(command);

    //    // Assert
    //    await Task.Delay(3000);
    //    var harness = Fixture.ServiceProvider.GetTestHarness();
    //    var eventWasPublished = await harness.Published.Any<CustomerCreated>();
    //    var consumed = await harness.Consumed.Any<CustomerCreated>();
    //    eventWasPublished.Should().Be(true);
    //    consumed.Should().Be(true);
    //}

    private static CreateCustomerCommand CreateValidCommand()
    {
        return new CreateCustomerCommand(
            "First",
            "Last",
            Guid.NewGuid() + "@test.com");
    }
}
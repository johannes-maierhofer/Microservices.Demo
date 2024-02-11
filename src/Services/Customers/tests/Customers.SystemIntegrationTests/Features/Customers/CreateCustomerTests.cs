using Customers.SystemIntegrationTests.Clients;
using FluentAssertions;

namespace Customers.SystemIntegrationTests.Features.Customers;

public class CreateCustomerTests : AppTestBase
{
    public CreateCustomerTests(AppTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnNonEmptyGuid_WhenCommandIsValid()
    {
        // Arrange
        var client = CreateCustomerClient();

        // Act
        var response = await client.CreateCustomerAsync(new CreateCustomerCommand
        {
            FirstName = "Herbert",
            LastName = "Hofer",
            EmailAddress = Guid.NewGuid() + "@aol.com"
        });

        // Assert
        response.Should().NotBeEmpty();
    }
}